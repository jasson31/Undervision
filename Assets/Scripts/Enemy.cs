using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;
    public Transform player;
    public float speed;
    public VisionType visionType;
    public EnemyType enemyType;
    public GameObject[] hearts;
    [SerializeField]
    int hp;
    [SerializeField]
    AudioSource audioSource;
    private float distRate;


    public void ChangeColor(VisionType _visionType)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", (int)_visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(_visionType));
        }
        visionType = _visionType;
        switch (_visionType)
        {
            case VisionType.Red: StartCoroutine(Illuminate()); break;
            case VisionType.Green: StartCoroutine(Vibrate()); break;
            case VisionType.Blue: StartCoroutine(Roar()); break;
            default: break;
        }
    }

    public IEnumerator Illuminate()
    {
        yield return null;
        //Illuminate
    }

    public IEnumerator Vibrate()
    {
        while (true)
        {
            yield return null;
            hapticAction.Execute(0, 0.02f, distRate * 200, distRate * 500, SteamVR_Input_Sources.LeftHand);
            hapticAction.Execute(0, 0.02f, distRate * 200, distRate * 500, SteamVR_Input_Sources.RightHand);
        }
    }

    public IEnumerator Roar()
    {
        audioSource.Play();
        while (true)
        {
            yield return null;
            audioSource.volume = distRate / 2;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        hp = hearts.Length;
    }
    void Update()
    {
        if (enemyType == EnemyType.Drone)
        {
            transform.position = transform.position - Vector3.Normalize(transform.position - player.position) * speed;
            transform.LookAt(player);
        }
        else
        {
            transform.Translate(Vector3.Normalize(transform.position - player.position) * speed);
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        }
        distRate = 1 - Vector3.Distance(player.position, transform.position) / EnemySpawner.enemySpawnDist;
    }
    public void Damaged()
    {
        hp--;
        for(int i = hp; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }
        if (hp <= 0) Killed();
    }
    public void Killed()
    {
        Destroy(gameObject);
    }
}
