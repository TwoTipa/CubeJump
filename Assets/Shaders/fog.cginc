#ifndef CUSTOM_FOG_INCLUDED
#define CUSTOM_FOG_INCLUDED

float3 FogOffset;
float3 FogAxis;
float FogScale;

float GetFog (float3 pos)
{
    float3 napr = pos * FogAxis;
    float fx = -(napr.x + napr.y + napr.z);

    return clamp((fx - FogOffset) * FogScale, 0.0, 1.0);
}

#endif