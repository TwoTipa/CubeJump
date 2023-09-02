Shader "LuB/ProjectorLub"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ColorProj ("Color", Color) = (0, 0, 0, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _ColorProj;

			float4x4 unity_Projector;
            float4x4 unity_ProjectorClip;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = mul(unity_Projector, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, (i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw)/i.uv.w);
				col *= _ColorProj;
				
				fixed d = dot(normalize(i.normal), -normalize(unity_Projector._m20_m21_m22));
				d = saturate(d);
				
				return fixed4(col.rgb, d * col.a);
			}
			ENDCG
		}
	}
}