Shader "Custom/ClearPass"
{
    SubShader
    {        
        Pass
        {
            ZTest Off
            ColorMask 0
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float3 normal : NORMAL;
//            };
//
//            struct v2f
//            {
//                float4 vertex : SV_POSITION;
//            };
//            
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = v.vertex;
//                o.vertex = UnityObjectToClipPos(o.vertex);
//                return o;
//            }
//            
//            fixed4 frag (v2f i) : SV_Target
//            {
//                return fixed4(0,0,0,1);
//            }
//            ENDCG
        }
    }
}