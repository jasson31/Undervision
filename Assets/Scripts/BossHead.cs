using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : Enemy
{
    float prevColorChangedTime = 0, prevMovedTime = 0;
    float colorChangeDelay = 10, moveDelay = 2;
    public GameObject headRedEffect;
    Coroutine bossPattern;

    public override void Update()
    {
        if (!GameManager.inst.gameOver)
        {
            transform.Translate(Vector3.Normalize(GameManager.inst.player.position - transform.position) * speed, Space.World);
            transform.LookAt(GameManager.inst.player);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        }
    }
    VisionType GetRandomColor()
    {
        List<VisionType> cands = new List<VisionType>();
        if (visionType != VisionType.Red) cands.Add(VisionType.Red);
        if (visionType != VisionType.Green) cands.Add(VisionType.Green);
        if (visionType != VisionType.Blue) cands.Add(VisionType.Blue);
        return cands[Random.Range(0, cands.Count)];
    }
    void ChangeBossHeadColor()
    {
        switch (visionType)
        {
            case VisionType.Red:
                headRedEffect.SetActive(false);
                break;
            case VisionType.Green:
                StopCoroutine(vibrationCoroutine);
                break;
            case VisionType.Blue:
                GetComponent<AudioSource>().Stop();
                break;
            default:
                break;
        }
        visionType = GetRandomColor();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", (int)visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(visionType));
            if (r.gameObject.tag.Contains("HeartFill")) r.material.SetColor("_Color", new Color(1, 0f, 0.8f));
        }
        switch (visionType)
        {
            case VisionType.Red:
                headRedEffect.SetActive(true);
                headRedEffect.transform.position = transform.position + new Vector3(0, 3, 0);
                break;
            case VisionType.Green:
                vibrationCoroutine = StartCoroutine(Vibrate());
                break;
            case VisionType.Blue:
                Roar();
                break;
        }
    }

    IEnumerator BossPattern()
    {
        yield return new WaitForSeconds(3f);
        GameManager.inst.closestGreenEnemy = this;
        headRedEffect.transform.parent = null;
        headRedEffect.SetActive(false);
        prevColorChangedTime = prevMovedTime = Time.time;

        while (!GameManager.inst.gameOver)
        {
            yield return null;
            if (Time.time - prevColorChangedTime >= colorChangeDelay)
            {
                ChangeBossHeadColor();
                prevColorChangedTime = Time.time;
            }

            if (Time.time - prevMovedTime >= moveDelay)
            {
                Vector3 originalPos = transform.position;

                float dist = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), GameManager.inst.player.position);
                float rad = Random.Range(-30f, 30f) * Mathf.Deg2Rad;
                Vector3 jumpPos = new Vector3(Mathf.Sin(rad) * dist, Random.Range(1f, 5f), Mathf.Cos(rad) * dist);
                float duration = 0.3f, x = 0;
                for (float timer = 0; timer <= duration; timer += Time.deltaTime)
                {
                    x = timer / duration;
                    transform.position = Vector3.Lerp(originalPos, jumpPos, x * x * x);
                    yield return null;
                }
                prevMovedTime = Time.time;
            }
        }
    }

    public override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        visionType = VisionType.White;
        ChangeBossHeadColor();
        bossPattern = StartCoroutine(BossPattern());
    }

    public override void GameOver()
    {
        StopCoroutine(bossPattern);
        base.GameOver();
    }

    public override void Damaged()
    {
        hp--;
        Instantiate(GameManager.inst.hitParticle, transform.position, Quaternion.identity).GetComponent<HitParticle>().SetColor(visionType);
        for (int i = hp; i < hearts.Length; i++) hearts[i].SetActive(false);
        colorChangeDelay -= 0.2f;
        if (hp <= 0)
        {
            GameManager.inst.gameOver = true;
            StartCoroutine(GameManager.inst.StageTextShow("CLEAR", -1));
        }
    }
}
