Shader "Custom/Window"
{
	Properties{
		_MaskType("Mask type", Range(0, 10)) = 0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		ZWrite Off
		ColorMask 0
		Pass {
			ZTest Less
			Stencil {
				 Ref [_MaskType]
				 Comp Always
				 Pass Replace
			}
		}
	}
}
