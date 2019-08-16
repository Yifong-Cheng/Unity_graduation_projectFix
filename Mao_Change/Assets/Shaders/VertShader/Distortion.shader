// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Distortion"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Noise("Noise", 2D) = "black" {}
		_DistortionMask("Distortion Mask", 2D) = "black" {}
		_DistortionStrength("Distortion Strength", Float) = 0.1
		_DistortTimeFactor("_Distort Time Factor", Float) = 0.1
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

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				sampler2D _MainTex;

				sampler2D _DistortionMask;
				sampler2D _Noise;
				float _DistortionStrength;
				float _DistortTimeFactor;

				fixed4 frag(v2f i) : SV_Target
				{
					//wave strength
					fixed strength = tex2D(_DistortionMask, i.uv).r;

				//noise
				fixed2 noi = tex2D(_Noise, i.uv - fixed2(0,_Time.y * _DistortTimeFactor));

				//uv offset
				fixed2 uvOffset = strength * noi.xy*_DistortionStrength;

				fixed4 col = tex2D(_MainTex, i.uv.xy + uvOffset.xy);

				return col;
			}
			ENDCG
		}
		}
}