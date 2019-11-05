using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public EnemySpawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coll " + other.gameObject.name);
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        spawner.GameOver();
        Debug.Log("GameOver");
    }
}
