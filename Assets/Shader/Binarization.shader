Shader "Custom/Binarization"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_SkinTex ("Skin Texture", 2D) = "white" {}
		//_GrayThreshold ("Gray Threshold", float) = 0.5
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
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			
			sampler2D _MainTex;
			//sampler2D _SkinTex;
			//float _GrayThreshold;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			inline fixed4 Rgb2Hsv(fixed4 rgba)
			{
				fixed4 K = fixed4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				fixed4 p = lerp(fixed4(rgba.bg, K.wz), fixed4(rgba.gb, K.xy), step(rgba.b, rgba.g));
				fixed4 q = lerp(fixed4(p.xyw, rgba.r), fixed4(rgba.r, p.yzx), step(p.x, rgba.r));
				
				fixed d = q.x - min(q.w, q.y);
				fixed e = 1.0e-6;
				return fixed4(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x, 0);
			}

			inline fixed4 Rgb2Yuv(fixed4 rgba)
			{
				fixed Y = 0.29900 * rgba.x + 0.57800 * rgba.y + 0.11400 * rgba.z;
				fixed Cb = -0.1687 * rgba.x - 0.3313 * rgba.y + 0.5 * rgba.z + 0.50196;
				fixed Cr = 0.5 * rgba.x - 0.4187 * rgba.y - 0.0813 * rgba.z + 0.50196;
				return fixed4(Y, Cb, Cr, 0);
			}

			//inline fixed InEllipse(fixed4 col)
			//{
			//	float2 uv = float2(col.y, col.z);
			//	return tex2D(_SkinTex, uv).x;
			//}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				//fixed gray = 0.2125 * col.r + 0.7154 * col.g + 0.0721 * col.b;
				//gray = step(gray, _GrayThreshold);
				//col = fixed4(gray, gray, gray, gray);
				//col = Rgb2Hsv(col);
				//col = fixed4(col.y, col.y, col.y, col.y);
				//fixed x = SkinBin(col);
				//col = Rgb2Yuv(col);
				//fixed x = InEllipse(col);
				//fixed x = (col.z < 0.62745 && col.z > 0.54902)? 1:0;
				//col = fixed4(x, x, x, 1);

				fixed3 cbC = fixed3(0.5, -0.4187, -0.0813);
				fixed3 crC = fixed3(-0.1687, -0.3313, 0.5);
				fixed x0 = col.x;
				fixed x1 = col.y;
				fixed x2 = col.z;
				fixed x3 = dot(cbC, col.xyz);
				fixed x4 = dot(crC, col.xyz);
				fixed pos = fixed((x4 <= -0.0615369)?
									((x3 <= 0.0678488)? 
									((x3 <= 0.0352417) ? 0 : (x2 <= 0.686631 ? 0 : 1)) : (x3 <= 0.185183 ? 1 : 0)) : 
									(x4 <=-0.029597 ? (x3 <= 0.0434402 ? 0 : (x1 <= 0.168271 ? 0 : 1)) : 0));
				col = fixed4(pos, pos, pos, 1);
				return col;
			}
			ENDCG
		}
	}
}
