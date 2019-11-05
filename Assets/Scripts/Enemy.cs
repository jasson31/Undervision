using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//delegate void EnemyEvent();
public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed;
    public VisionType visionType;
    public EnemyType enemyType;
    public GameObject[] hearts;
    public EnemySpawner spawner;
    [SerializeField]
    int hp;
    [SerializeField]
    AudioSource audioSource;
    bool gameOver;

    //event EnemyEvent enemyEvent;

    public void ChangeColor(VisionType _visionType)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", (int)_visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(_visionType));
            if (r.gameObject.tag.Contains("HeartFill")) r.material.SetColor("_Color", new Color(1, 0.75f, 0.8f));
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
        //Vibrate start;
        while (true)
        {
            yield return null;
            //Vibrate amount increase;
        }
    }

    public IEnumerator Roar()
    {
        audioSource.Play();
        while (true)
        {
            yield return null;
            audioSource.volume = (1 - Vector3.Distance(player.position, transform.position) / EnemySpawner.enemySpawnDist) / 2;
        }
    }

    private void Start()
    {
        hp = hearts.Length;
    }
    void Update()
    {
        if (!gameOver)
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
        }
    }
    public void Damaged()
    {
        if (!gameOver)
        {
            hp--;
            for (int i = hp; i < hearts.Length; i++)
            {
                hearts[i].SetActive(false);
            }
            if (hp <= 0) Killed();
        }
    }
    public void Killed()
    {
        spawner.EnemyDead(gameObject);
        Destroy(gameObject);
    }
    public void GameOver()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", 0);
        }
        gameOver = true;
        if(GetComponent<Animator>()) GetComponent<Animator>().enabled = false;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic = true;
    }
}
