#ifndef UNLIT_PASS_INCLUDED
#define UNLIT_PASS_INCLUDED

#include "Assets/RMRP/ShaderLib/Common.hlsl"

struct Input
{
    float4 position : POSITION;
    float4 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Output
{
    float4 posCS : SV_POSITION;
    float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float4,_BaseColor)
    UNITY_DEFINE_INSTANCED_PROP(float,_Cutoff)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)
#define IMPS(property) UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, property)

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

Output UnlitPassVertex(Input i)
{
    Output o;

    UNITY_SETUP_INSTANCE_ID(i);
    UNITY_TRANSFER_INSTANCE_ID(i,o);

    float3 posWS = TransformObjectToWorld(i.position.xyz);
    o.posCS = TransformWorldToHClip(posWS);

    o.uv = i.uv * IMPS(_BaseMap_ST).xy + IMPS(_BaseMap_ST).zw;
    return o;
}

float4 UnlitPassFragment(Output i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    float4 baseTexture = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
    #if defined(_CLIPPING)
        clip(baseTexture * IMPS(_BaseColor).a - IMPS(_Cutoff));
    #endif
    return baseTexture * IMPS(_BaseColor);
}


#endif

