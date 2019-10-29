using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.Normalize(transform.position - player.position) * speed);
        transform.LookAt(player);
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);

    }
}
