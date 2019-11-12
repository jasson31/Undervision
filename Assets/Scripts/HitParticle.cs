using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    ParticleSystem ps;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.collision.SetPlane(0, GameManager.inst.floor);
    }
    public void SetColor(Color color)
    {
        Renderer r = ps.GetComponent<Renderer>();
        r.material.SetInt("_MaskType", 0);
        r.material.SetColor("_Color", color);
        r.material.SetInt("_StencilComp", 0);
    }
    public void SetColor(VisionType visionType)
    {
        Renderer r = ps.GetComponent<Renderer>();
        r.material.SetInt("_MaskType", 0);
        r.material.SetColor("_Color", Constants.Vision_Color(visionType));
        r.material.SetInt("_StencilComp", 0);
    }
}
