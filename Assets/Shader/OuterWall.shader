Shader "Custom/OuterWall"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		Cull Front
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			fixed4 _Color;
			struct appdata {
				float4 vertex : POSITION;
			};
			struct v2f {
				float4 pos : SV_POSITION;
			};
			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}
			half4 frag(v2f i) : SV_Target {
				return _Color;
			}
			ENDCG
		}
	}
}
