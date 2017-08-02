Shader "Custom/PaththoughEffect"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "black" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Fog {Mode Off}

		Pass
		{
			CGPROGRAM
			#pragma fragmentoption ARB_precision_hint_fastest;
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _NoiseTex;
			uniform float _DistortFactor;
			uniform float4 _DistortCenter;
			uniform float _DistortStrength;

			fixed4 frag(v2f_img i) : SV_Target
			{
				//计算偏移的方向  
				float2 dir = i.uv - _DistortCenter.xy;
				//最终偏移的值：方向 * （1-长度），越靠外偏移越小
				float2 scaleOffset = _DistortFactor * normalize(dir) * (1-length(dir));
				//采样Noise贴图
				float2 noise = tex2D(_NoiseTex, i.uv);
				//noise的权重 = 参数 * 距离，越靠近外边的部分，扰动越严重
				float2 noiseOffset = noise.xy * _DistortStrength * dir;
				//计算最终offset = 两种扭曲offset的差（取和也行，总之效果好是第一位的）
				float2 offset = scaleOffset - noiseOffset;
				float2 uv = i.uv + offset;
				return tex2D(_MainTex, uv);
			}

			ENDCG
		}
	}
}
