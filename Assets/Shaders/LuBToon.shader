Shader "LuB/NewToon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _LitTrashHold ("Lit Trashhold", Range(-1,1)) = 0
        _LitSoftness ("Lit Softness", Range(0,1)) = 0
        _ShadingColor ("Shading Color", Color) = (1,1,1,1)
        [Toggle(USE_SPECULAR)] _UseSpecular ("Use Specular", Float) = 0
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _SpecularSize ("Specular Size", Range(0,1)) = 0
        _SpecularSoftness ("Specular Softness", Range(0,1)) = 0
        [Toggle(USE_FOG)] _UseFog ("Use fog", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing
            #pragma multi_compile_fwdadd_fullshadows
            #pragma shader_feature USE_FOG
            #pragma shader_feature USE_SPECULAR

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD3;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 worldPos : TEXCOORD4;
                unityShadowCoord4 _ShadowCoord : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;

            float _LitTrashHold;
            float _LitSoftness;
            fixed4 _ShadingColor;
            fixed4 _Highlight;
            
            fixed4 _SpecularColor;
            float _SpecularSize;
            float _SpecularSoftness;

            #ifdef USE_FOG
            #include "skybox.cginc"
            #include "fog.cginc"
            #endif

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = v.uv2;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * lerp(_Color, _Highlight, _Highlight.a);

                float fong = dot(_WorldSpaceLightPos0.xyz, normalize(i.worldNormal));
                float natFong = fong;

                #ifdef USE_SPECULAR
                fixed3 viewVec = normalize(i.worldPos - _WorldSpaceCameraPos);
                float spec = pow(max(0.0, dot(
      reflect(normalize(_WorldSpaceLightPos0), normalize(i.worldNormal)), 
      viewVec)), 32.0 * _SpecularSize);
                #else
                float spec = 0;
                #endif

                fong = smoothstep(_LitTrashHold - _LitSoftness*2, _LitTrashHold + _LitSoftness, fong);

                fong = saturate(fong);

                fixed4 retColor = col * lerp(1, LIGHT_ATTENUATION(i), max(natFong, 0)) * max(_ShadingColor, fong) + spec * _SpecularColor * _SpecularColor.a;

                #ifdef USE_FOG
                fixed3 fogCoord = normalize(i.worldPos - _WorldSpaceCameraPos);
                retColor = lerp(retColor, GetSkyboxColor(fogCoord), GetFog(i.worldPos));
                #endif
                
                return retColor;
            }
            ENDCG
        }
        
        UsePass "VertexLit/SHADOWCASTER"
    }
}
