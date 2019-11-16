Shader "Custom/WindowFirst"
{
	Properties
	{
		_MaskType("Mask type", Range(0, 10)) = 0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		ColorMask 0
		Pass {
			Stencil {
				Ref[_MaskType]
				Pass Replace
			}
		}
	}
}