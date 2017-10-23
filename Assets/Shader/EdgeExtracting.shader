Shader "Custom/EdgeExtracting"
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
				float4 pos : SV_POSITION;
				half2 uv[9] : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

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

			float luminance(float3 color)
            {
                return dot(fixed3(0.2125, 0.7154, 0.0721), color);
            }
 
            half sober(half2 uvs[9])
            {
                half gx[9] = {-1, 0, 1,
                              -2, 0, 2,
                              -1, 0, 1};
                half gy[9] = {-1, -2, -1,
                               0,  0,  0,
                               1,  2,  1};
 
                float edgeX = 0;
                float edgeY = 0;
 
                for(int i = 0; i < 9; i++)
                {
                    fixed3 c = tex2D(_MainTex, uvs[i]).rgb;
                    float l = luminance(c);
                    edgeX += l * gx[i];
                    edgeY += l * gy[i];
                }
 
                return abs(edgeX) + abs(edgeY);
            }


			fixed4 frag (v2f i) : SV_Target
			{
				////fixed col[9];
				////col[0] = tex2D(_MainTex, i.uv[0]).x;
				////col[1] = tex2D(_MainTex, i.uv[1]).x;
				////col[2] = tex2D(_MainTex, i.uv[2]).x;
				////col[3] = tex2D(_MainTex, i.uv[3]).x;
				////col[4] = tex2D(_MainTex, i.uv[4]).x;
				////col[5] = tex2D(_MainTex, i.uv[5]).x;
				////col[6] = tex2D(_MainTex, i.uv[6]).x;
				////col[7] = tex2D(_MainTex, i.uv[7]).x;
				////col[8] = tex2D(_MainTex, i.uv[8]).x;
				////fixed conv = col[0] * _Kernel3x3[0][0] + col[1] * _Kernel3x3[0][1] + col[2] * _Kernel3x3[0][2] +
				////			 col[3] * _Kernel3x3[1][0] + col[4] * _Kernel3x3[1][1] + col[5] * _Kernel3x3[1][2] +
				////			 col[6] * _Kernel3x3[2][0] + col[7] * _Kernel3x3[2][1] + col[6] * _Kernel3x3[2][2];
				//////return fixed4(col[4], col[4], col[4], 1);
				//conv *= 0.8;
				//return fixed4(conv, conv, conv, 1);
				half edge = sober(i.uv);
				fixed4 tex = tex2D(_MainTex, i.uv[4]);
				return fixed4(edge, edge, edge, 1);
			}
			ENDCG
		}
	}
}
