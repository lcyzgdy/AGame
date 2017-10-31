// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Nature/GrassShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TimeScale ("Time Scale", float) = 1
	}

	CGINCLUDE
		#define UNITY_SETUP_BRDF_INPUT MetallicSetup
	ENDCG

	SubShader
	{
		Tags 
		{
			"RenderType" = "Opaque"
			"Queue" = "Transparent"
			"CanUseSpriteAtlas" = "True"
		}
		LOD 100

		Pass
		{
			Tags
			{
				"LightMode" = "ForwardBase"
			}
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off
			Lighting On
			Offset -1, -1
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#pragma multi_compile_fwdbase'
			
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				LIGHTING_COORDS(3, 4)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _TimeScale;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 offset = float4(0, 0, 0, 0);
				offset.x = sin(3.14159 * _Time.y * clamp(v.uv.y - 0.5, 0, 1)) * _TimeScale;
				o.pos = UnityObjectToClipPos(v.pos + offset);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o, o.pos);
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 lightColor = _LightColor0.rgb;
				fixed3 lightDir = _WorldSpaceLightPos0;
				// sample the texture
				fixed4 colorTex = tex2D(_MainTex, i.uv);
				fixed atten = LIGHT_ATTENUATION(i);
				fixed3 n = fixed3(0.0f, 1.0f, 0.0f);
				fixed3 nl = saturate(dot(n, lightDir));
				//fixed4 col = tex2D(_MainTex, i.uv);
				fixed3 col = colorTex.rgb * lightColor * nl * atten;
				fixed4 finalCol = fixed4(col, colorTex.a);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, finalCol);
				return finalCol;
			}
			ENDCG
		}

		Pass
		{
			Name "ShadowCaster"

			Tags
			{
				"LightMode" = "ShadowCaster" 
			}

			ZWrite On
			ZTest LEqual

			CGPROGRAM
			#pragma target 2.0

			#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma skip_variants SHADOWS_SOFT
			#pragma multi_compile_shadowcaster

			#pragma vertex vertShadowCaster
			#pragma fragment fragShadowCaster

			#include "UnityStandardShadow.cginc"

			ENDCG
		}
	}
}
