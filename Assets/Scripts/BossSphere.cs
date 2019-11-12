using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSphere : Enemy
{
    public BossCrystal.Balloon balloon;
    public override void Damaged()
    {
        Instantiate(GameManager.inst.hitParticle, transform.position, Quaternion.identity).GetComponent<HitParticle>().SetColor(balloon.visionType);
        balloon.Destroyed();
    }

    public override void Start()
    {

    }

    public override void Update()
    {
        
    }
}
