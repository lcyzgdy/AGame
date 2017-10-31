Shader "Custom/HilditchSkeleton"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv[9] : TEXCOORD0;
				float4 pos : SV_POSITION;
			};
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.pos);
				o.uv[0] = v.uv + _MainTex_TexelSize.xy * half2(-1, 1);
				o.uv[1] = v.uv + _MainTex_TexelSize.xy * half2(0, 1);
				o.uv[2] = v.uv + _MainTex_TexelSize.xy * half2(1, 1);
				o.uv[3] = v.uv + _MainTex_TexelSize.xy * half2(-1, 0);
				o.uv[4] = v.uv + _MainTex_TexelSize.xy * half2(0, 0);
				o.uv[5] = v.uv + _MainTex_TexelSize.xy * half2(1, 0);
				o.uv[6] = v.uv + _MainTex_TexelSize.xy * half2(-1, -1);
				o.uv[7] = v.uv + _MainTex_TexelSize.xy * half2(0, -1);
				o.uv[8] = v.uv + _MainTex_TexelSize.xy * half2(1, -1);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv4]);
				// just invert the colors
				
				return col;
			}
			ENDCG
		}
	}
}
