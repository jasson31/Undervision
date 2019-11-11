using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSphere : Enemy
{
    public BossCrystal.Balloon balloon;
    public override void Damaged()
    {
        balloon.Destroyed();
    }

    public override void Start()
    {

    }

    public override void Update()
    {
        
    }
}
