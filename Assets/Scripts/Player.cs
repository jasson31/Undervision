using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public EnemySpawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        if(!spawner.gameOver && other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            GameObject.Find("OuterWall").GetComponent<Renderer>().material.SetColor("_Color", Constants.Vision_Color(other.GetComponent<Enemy>().visionType));
            GameOver();
            //other.GetComponent<Enemy>().Killed();
        }
    }

    void GameOver()
    {
        spawner.GameOver();
        Debug.Log("GameOver");
    }
}
