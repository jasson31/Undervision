using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : Enemy
{
    float prevChangedTime = 0;
    float colorChangeDelay = 10;
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
        int random = (int)Time.time % 2 == 0 ? 1 : -1;
        Debug.Log(random);
        int newVisionType = (int)visionType + random;
        if (newVisionType < 0) newVisionType = 3 + newVisionType;
        if (newVisionType == 0) newVisionType = newVisionType + Random.Range(1, 3);
        return (VisionType)newVisionType;
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

    IEnumerator BossColorPattern()
    {
        /*for (float timer = 0; timer <= 3f; timer += Time.deltaTime)
        {
            yield return null;
        }*/
        headRedEffect.transform.parent = null;
        headRedEffect.SetActive(false);
        prevChangedTime = Time.time;


        float xLimit, angle = 0;

        Coroutine moving = StartCoroutine(BossMovePattern());
        while (!GameManager.inst.gameOver)
        {
            yield return null;
            if (Time.time - prevChangedTime >= colorChangeDelay)
            {
                ChangeBossHeadColor();
                prevChangedTime = Time.time;
            }

            /*xLimit = Vector3.Distance(transform.position, GameManager.inst.player.position) * 5 / 12;

            angle += Time.deltaTime;
            if (angle == Mathf.PI * 2) angle = 0;
            float sin = Mathf.Sin(angle);

            transform.position = new Vector3(sin * xLimit * 0.5f, transform.position.y, transform.position.z);*/

        }
        StopCoroutine(moving);
    }





    IEnumerator BossMovePattern()
    {

        float dist = Vector3.Distance(transform.position, GameManager.inst.player.position);
        float rad = Random.Range(-20f, 20f) * Mathf.PI / 180f + Mathf.Atan2(transform.position.z, transform.position.x);
        if(rad > 20) rad = 20
        Vector3 jumpPos = new Vector3(Mathf.Cos(rad) * dist, 0, Mathf.Sin(rad) * dist);




        float rad = degrees * Mathf.PI / 180f;
        temp.transform.position = new Vector3(Mathf.Sin(rad) * dist, _enemyType == EnemyType.Drone ? 5 : 0, Mathf.Cos(rad) * dist);
        temp.ChangeColor(_visionType);
        while (!GameManager.inst.gameOver)
        {
            yield return null;
            if (Time.time - prevChangedTime >= colorChangeDelay)
            {
                ChangeBossHeadColor();
                prevChangedTime = Time.time;
            }

        }
    }





    public override void Start()
    {
        audioSource = GetComponent<AudioSource>();
        visionType = VisionType.White;
        ChangeBossHeadColor();
        bossPattern = StartCoroutine(BossColorPattern());
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
