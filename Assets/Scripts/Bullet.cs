using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
            collision.gameObject.GetComponent<Enemy>().Damaged();
        Destroy(gameObject);
    }
}
