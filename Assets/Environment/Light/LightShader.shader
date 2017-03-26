Shader "Custom/LightShader" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent"
		       "Queue" = "Transparent" }
		LOD 200

		Pass {

			// Don't write to the depth buffer.
			ZWrite Off
			// Don't write pixels we have already written.
			//ZTest Less

			Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending

			CGPROGRAM
			#pragma vertex vert             
			#pragma fragment frag
        		#include "UnityCG.cginc"

			struct vertInput {
				float2 uv : TEXCOORD0;
				float4 pos : POSITION;
			};  

			struct vertOutput {
				float2 uv : TEXCOORD0;
				float wposx : WORLD_X;
				float4 pos : SV_POSITION;
			};

			float4 _MainTex_ST;

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.wposx = input.pos.x;
				o.pos = mul(UNITY_MATRIX_MVP, input.pos);
            			o.uv = TRANSFORM_TEX (input.uv, _MainTex);
				return o;
			}

			sampler2D _MainTex;
			uniform float4 _Color;
			uniform float _MaxX;

			fixed4 frag(vertOutput output) : COLOR {
				clip(_MaxX - output.wposx);
				fixed4 c = tex2D (_MainTex, output.uv) * _Color;
				return c; 
			}
			ENDCG
		}
	}
}
