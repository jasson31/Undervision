using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    public VisionType visionType;
    public Renderer panelRenderer;
    public Renderer[] frameRenderers;

    public void ChangeColor(VisionType _visionType)
    {
        foreach (Renderer r in frameRenderers)
        {
            r.material.SetColor("_Color", Constants.Vision_Color(_visionType));
        }
        panelRenderer.material.SetInt("_MaskType", (int)_visionType);
        panelRenderer.material.renderQueue = 1950 + (int)_visionType;
        visionType = _visionType;
    }

    private void Start()
    {
        ChangeColor(visionType);
    }
}
