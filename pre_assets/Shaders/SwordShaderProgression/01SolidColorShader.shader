Shader "Custom/01SolidColorShader" {
	SubShader{
		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			struct vertInput {
				float4 pos : POSITION;
			};

			struct vertOutput {
				float4 pos : SV_POSITION;
			};

			vertOutput vert(vertInput input) {
				vertOutput o;
				o.pos = UnityObjectToClipPos(input.pos);
				return o;
			}

			half4 frag(vertOutput output) : COLOR{
				//return the rgba values inside of a half4
				return half4(1.0, 0.0, 0.0, 1.0);
			}
			ENDCG
		}
	}
}
