Shader "Custom/ObjOutline"
{
    SubShader
    {
        Stencil {
            Ref 2
            Comp Always
            Pass Replace
        }
        
        Pass
        {
            Cull Off
            
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
                float4 vertex : SV_POSITION;
            };

            float4 _ColorOut;
            float _OutWidth;
            int _UseScale;
            
            v2f vert (appdata v)
            {
                v2f o;
                if (_UseScale == 1)
                {
                    o.vertex = v.vertex * _OutWidth;
                } else
                {
                    o.vertex = v.vertex;
			        o.vertex.xyz += normalize(v.normal.xyz) * _OutWidth * 0.008;
                }
                //o.vertex = v.vertex * _OutWidth;
                o.vertex = UnityObjectToClipPos(o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                return _ColorOut;
            }
            ENDCG
        }
    }
}