using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed;
    public VisionType visionType;
    public EnemyType enemyType;
    public GameObject[] hearts;
    int hp;

    public void ChangeColor(VisionType _visionType)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", (int)_visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(_visionType));
        }
        visionType = _visionType;
    }

    private void Start()
    {
        hp = hearts.Length;
    }
    void Update()
    {
        if(enemyType == EnemyType.Drone)
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
