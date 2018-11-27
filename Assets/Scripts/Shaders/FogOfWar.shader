Shader "Hidden/FogOfWar"
{
	HLSLINCLUDE

			#include "PostProcessing/Shaders/StdLib.hlsl"
			TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
			TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
			float4x4 _inverseView;
			float4 _VisionPoints[8];
			int _VisionUnit;
			float4 _fogColor;
			float _density;

			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
				float depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord));
				
				float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
				float3 wPos = float3((i.texcoord * 2 - 1) / p11_22, -1) * depth;

				wPos = mul(_inverseView, float4(wPos, 1)).xyz;

				depth *= 0.01;

				float fog = 0.0f;

				for (int i = 0; i < _VisionUnit; i++)
				{
					float distToCenter = length(wPos - _VisionPoints[i].xyz);
					float circle = _VisionPoints[i].w - distToCenter;

					fog = max(fog, circle);
				}
				fog = smoothstep(0.6, 1.0, fog);

				float minFog = 1.0 - _density;

				return lerp(_fogColor, color, max(fog, minFog));
			}

	ENDHLSL

	SubShader
	{
		Pass
		{
			Cull Off ZWrite Off ZTest Always
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}