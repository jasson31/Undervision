using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBehavior : GrabbableObject
{
    private void Update()
    {
        ToTable();
    }
    private void Start()
    {
        SetTable();
    }
    public override void HairTrigger()
    {
        
    }
}
