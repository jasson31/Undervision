Shader "Custom/Window"
{
	Properties{
		_StencilMask("Mask Layer", Range(0, 255)) = 1
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		ZWrite Off
		ColorMask 0
		Pass {
			Stencil {
				 Ref 255
				 WriteMask[_StencilMask]
				 Comp Always
				 Pass Replace
			}
		}
	}
}
