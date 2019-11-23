using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;
    public float speed;
    public VisionType visionType;
    public EnemyType enemyType;
    public GameObject[] hearts;
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected AudioSource audioSource;
    private float distRate;
    public GameObject redEffect;
    protected Coroutine vibrationCoroutine;

    public virtual void ChangeColor(VisionType _visionType)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", (int)_visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(_visionType));
            if(_visionType == VisionType.White) r.material.SetInt("_StencilComp", 0);
            if (r.gameObject.tag.Contains("HeartFill")) r.material.SetColor("_Color", new Color(1, 0f, 0.8f));
        }
        visionType = _visionType;
        switch (_visionType)
        {
            case VisionType.Red: Illuminate(); break;
            case VisionType.Green: vibrationCoroutine = StartCoroutine(Vibrate()); break;
            case VisionType.Blue: Roar(); break;
            default: break;
        }
    }

    public void Illuminate()
    {
        float rad = Random.Range(-20f, 20f) * Mathf.PI / 180f + Mathf.Atan2(transform.position.z, transform.position.x);
        Vector3 redEffectPos = new Vector3(Mathf.Cos(rad) * 19.5f, 0, Mathf.Sin(rad) * 19.5f);
        redEffect = Instantiate(redEffect, redEffectPos + new Vector3(0, Random.Range(5, 8), 0), Quaternion.identity);
    }

    public IEnumerator Vibrate()
    {
        while (true)
        {
            yield return null;
            if (GameManager.inst.gameOver) break;
            else if(GameManager.inst.closestGreenEnemy == this)
            {
                SteamVR_Input_Sources vibrateHand = Vector3.Distance(GameManager.inst.leftH.transform.position, transform.position) >
                    Vector3.Distance(GameManager.inst.rightH.transform.position, transform.position) ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
                hapticAction.Execute(0, 0.02f, 200, 500, vibrateHand);
            }
        }
    }

    public void Roar()
    {
        audioSource.Play();
    }

    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hp = hearts.Length;
    }
    public virtual void Update()
    {
        if (!GameManager.inst.gameOver)
        {
            if (enemyType == EnemyType.Drone)
            {
                transform.Translate(Vector3.Normalize(GameManager.inst.player.position - transform.position) * speed, Space.World);
                transform.LookAt(GameManager.inst.player);
            }
            else
            {
                transform.Translate(Vector3.Normalize(GameManager.inst.player.position - transform.position) * speed, Space.World);
                transform.LookAt(GameManager.inst.player);
                transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            }
            if(visionType == VisionType.Green)
            {
                if(GameManager.inst.closestGreenEnemy == null || Vector3.Distance(transform.position, GameManager.inst.player.position)
                    < Vector3.Distance(GameManager.inst.closestGreenEnemy.transform.position, GameManager.inst.player.position))
                GameManager.inst.closestGreenEnemy = this;
            }
            distRate = 1 - Vector3.Distance(GameManager.inst.player.position, transform.position) / GameManager.enemySpawnDist;
        }
    }
    public virtual void Damaged()
    {
        if (!GameManager.inst.gameOver)
        {
            hp--;
            Instantiate(GameManager.inst.hitParticle, transform.position, Quaternion.identity).GetComponent<HitParticle>().SetColor(visionType);
            for (int i = hp; i < hearts.Length; i++)
            {
                hearts[i].SetActive(false);
            }
            if (hp <= 0) Killed();
        }
    }
    public virtual void Killed()
    {
        if (visionType == VisionType.Red) Destroy(redEffect);
        GameManager.inst.EnemyDead(gameObject);
        Destroy(gameObject);
    }
    public virtual void GameOver()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_StencilComp", 0);
        }
        if (GetComponent<Animator>()) GetComponent<Animator>().enabled = false;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;
        switch (visionType)
        {
            case VisionType.Red:
                break;
            case VisionType.Green:
                StopCoroutine(vibrationCoroutine);
                break;
            case VisionType.Blue:
                audioSource.Stop();
                break;
        }
    }
}
