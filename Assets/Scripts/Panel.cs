using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public VisionType visionType;
    public Renderer panelRenderer;
    public Renderer[] frameRenderers;

    private void Start()
    {
        foreach (Renderer r in frameRenderers)
        {
            r.material.SetColor("_Color", Constants.Vision_Color(visionType));
        }
        panelRenderer.material.SetInt("_StencilMask", (int)visionType);
    }
}
