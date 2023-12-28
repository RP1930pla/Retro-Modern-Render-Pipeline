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
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4,_BaseColor)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)
#define IMPS(property) UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, property)

Output UnlitPassVertex(Input i)
{
    Output o;

    UNITY_SETUP_INSTANCE_ID(i);
    UNITY_TRANSFER_INSTANCE_ID(i,o);

    float3 posWS = TransformObjectToWorld(i.position.xyz);
    o.posCS = TransformWorldToHClip(posWS);
    return o;
}

float4 UnlitPassFragment(Output i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    return IMPS(_BaseColor);
}


#endif

