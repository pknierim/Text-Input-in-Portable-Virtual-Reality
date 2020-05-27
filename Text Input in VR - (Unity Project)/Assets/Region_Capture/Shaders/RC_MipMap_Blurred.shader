Shader "Unlit/RegionCapture/MipMap Blurred"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_mipMapBias("Mip-Map Bias", Float) = 0.0
		_transparency ("Transparency", Range (0,1.0)) = 1.0
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		Fog { Mode Off }

		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"

			#pragma glsl 
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest

			sampler2D _MainTex;
			half _mipMapBias;
			half _transparency;

			struct v2f
			{
				half4 pos : SV_POSITION;
				half2 uv  : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv  = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				fixed4 c = tex2Dbias(_MainTex, float4(i.uv.x, i.uv.y, 0.0, _mipMapBias));
				c.a *= _transparency;
				return c;
			}
			ENDCG
		}
	}
	FallBack off
}