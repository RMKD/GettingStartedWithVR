// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FadeShader"
{
	Properties
	{
		_Color ("Color", Color) = (0,0,0,1)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Cull Off // this makes the textures double sided
		Blend SrcAlpha OneMinusSrcAlpha // this sets up the way to combine layers to apply the transparency
		LOD 100 //Level of Detail

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float _alpha;

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

			half4 frag(vertOutput output) : COLOR {
				return half4(0.0, 0.0, 0.0, _alpha); 
			}
			ENDCG
		}
	}
}
