Shader "Custom/windowb"
{
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		ZWrite Off
		ColorMask 0
		Pass {
			Stencil {
				Ref 2
				Comp always
				Pass replace
			}
		}
	}
}
