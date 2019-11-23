using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : Enemy
{
    public override void Damaged()
    {
        GameManager.inst.stageStart = true;
        gameObject.SetActive(false);
    }
    public override void Start()
    {
        ChangeColor(VisionType.Red);
    }
    public override void Update()
    {

    }
    public override void ChangeColor(VisionType _visionType)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.SetInt("_MaskType", (int)_visionType);
            r.material.SetColor("_Color", Constants.Vision_Color(_visionType));
        }
        visionType = _visionType;
    }
}
