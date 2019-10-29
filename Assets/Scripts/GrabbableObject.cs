using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableObject : MonoBehaviour
{
    public Vector3 pos;
    public Vector3 angle;
    public abstract void HairTrigger();
}
