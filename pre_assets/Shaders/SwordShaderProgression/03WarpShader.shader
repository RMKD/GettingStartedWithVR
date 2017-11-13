Shader "Custom/03WarpShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_WarpTex ("Warp Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Range(0, 1)) = 0.1
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
				float2 uv : TEXCOORD1;
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
			sampler2D _WarpTex;
			float _Magnitude;
			float _Speed;
			fixed4 frag (vertOutput i) : SV_Target
			{

				float2 distuv = float2(i.uv.x, i.uv.y);
				float2  disp = tex2D(_WarpTex, distuv).xy;
				disp = ((disp * 2) -1) * _Magnitude;

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv + disp);
				return col;
			}
			ENDCG
		}
	}
}
