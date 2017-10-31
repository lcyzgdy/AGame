Shader "Custom/Dilation"
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
			float4x4 _Kernel;

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
				fixed b[9];
				b[0] = tex2D(_MainTex, i.uv[0]).x;
				b[1] = tex2D(_MainTex, i.uv[1]).x;
				b[2] = tex2D(_MainTex, i.uv[2]).x;
				b[3] = tex2D(_MainTex, i.uv[3]).x;
				b[4] = tex2D(_MainTex, i.uv[4]).x;
				b[5] = tex2D(_MainTex, i.uv[5]).x;
				b[6] = tex2D(_MainTex, i.uv[6]).x;
				b[7] = tex2D(_MainTex, i.uv[7]).x;
				b[8] = tex2D(_MainTex, i.uv[8]).x;

				fixed sum = _Kernel[0][0] * b[0] + _Kernel[0][1] * b[1] + _Kernel[0][2] * b[2] +
							_Kernel[1][0] * b[3] + _Kernel[1][1] * b[4] + _Kernel[1][2] * b[5] +
							_Kernel[2][0] * b[6] + _Kernel[2][1] * b[7] + _Kernel[2][2] * b[8];
				fixed bin = step(2, sum);
				return fixed4(bin, bin, bin, 1);
			}
			ENDCG
		}
	}
}
