using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            other.GetComponent<Enemy>().Damaged();
            Destroy(gameObject);
        }
        else if (other.tag.Equals("OuterWall"))
        {
            Destroy(gameObject);
        }
    }
}
