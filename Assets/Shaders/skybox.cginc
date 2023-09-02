#ifndef CUSTOM_SKYBOX_INCLUDED
#define CUSTOM_COMMON_INCLUDED
#include <HLSLSupport.cginc>

fixed3 TopColorSkybox;
fixed3 BottomColorSkybox;
float ExpSkybox;
float3 UpSkybox;

fixed4 GetSkyboxColor(float3 coord)
{
    float3 up = normalize(UpSkybox);
    float d = dot(coord, up);
    return fixed4(lerp(BottomColorSkybox, TopColorSkybox, sign(d) * pow(abs(d), ExpSkybox) * 0.5 + 0.5), 1);    
}

#endif