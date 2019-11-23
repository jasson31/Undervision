using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : Enemy
{
    public override void Damaged()
    {
        Debug.Log("fuck");
        GameManager.inst.stageStart = false;
    }
    public override void Start()
    {

    }
    public override void Update()
    {

    }
}
