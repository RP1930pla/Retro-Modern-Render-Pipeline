#ifndef MODERN_LIGHTING_INCLUDED
#define MODERN_LIGHTING_INCLUDED

//#include "Assets/RMRP/ShaderLib/Surface.hlsl"
#include "Assets/RMRP/ShaderLib/Light.hlsl"

float3 IncomingLight(Surface surface, Light light)
{
    return saturate(dot(surface.normal, light.direction)) * light.color;
}

float3 GetLighting(Surface surface, Light light)
{
    return IncomingLight(surface,light) * surface.color;
}

float3 GetLighting (Surface surface)
{
    return GetLighting(surface, GetDirectionalLight());
}

#endif