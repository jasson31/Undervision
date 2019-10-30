using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed;
    public VisionType visionType;

    private void Start()
    {
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_StencilMask", (int)visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(visionType));
        }
    }
    void Update()
    {
        transform.Translate(Vector3.Normalize(transform.position - player.position) * speed);
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);

    }
}
