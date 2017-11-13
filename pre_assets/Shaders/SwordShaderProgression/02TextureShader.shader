Shader "Custom/02TextureShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct vertInput
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD1; //we use 1 here instead of 0 as the model export has the UV map in TEXCOORD1
			};

			struct vertOutput
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};



			vertOutput vert (vertInput v)
			{
				vertOutput o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			fixed4 frag (vertOutput i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
