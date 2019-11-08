using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(!GameManager.inst.gameOver && other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            GameManager.inst.outerWall.GetComponent<Renderer>().material.SetColor("_Color", Constants.Vision_Color(other.GetComponent<Enemy>().visionType));
            GameOver();
            //other.GetComponent<Enemy>().Killed();
        }
    }

    void GameOver()
    {
        GameManager.inst.GameOver();
        Debug.Log("GameOver");
    }
}
