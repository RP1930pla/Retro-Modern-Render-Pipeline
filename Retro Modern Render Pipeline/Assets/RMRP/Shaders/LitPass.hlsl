#ifndef MODERNLIT_PASS_INCLUDED
#define MODERNLIT_PASS_INCLUDED

#include "Assets/RMRP/ShaderLib/Common.hlsl"
#include "Assets/RMRP/ShaderLib/RetroFX.hlsl"

#include "Assets/RMRP/ShaderLib/Surface.hlsl"
#include "Assets/RMRP/ShaderLib/ModernLighting.hlsl"

struct Input
{
    float4 position : POSITION;
    float3 normalOS : NORMAL;
    float4 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Output
{
    float4 posCS : SV_POSITION;
    float3 normalWS : TEXCOORD3;
    AFFINE_UV float2 uv : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

//#region shader buffer properties 
UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float4,_BaseColor)
    UNITY_DEFINE_INSTANCED_PROP(float,_Cutoff)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)
///Access buffer property
#define IMPS(property) UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, property)
//#endregion

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

Output LitPassVertex(Input i)
{
    Output o;

    UNITY_SETUP_INSTANCE_ID(i);
    UNITY_TRANSFER_INSTANCE_ID(i,o);

    float3 posWS = TransformObjectToWorld(i.position.xyz);
    o.posCS = TransformWorldToHClip(posWS);
    SNAP_TO_PIXEL(o.posCS);

    o.normalWS = TransformObjectToWorldNormal(i.normalOS);

    o.uv = i.uv * IMPS(_BaseMap_ST).xy + IMPS(_BaseMap_ST).zw;
    return o;
}

float4 LitPassFragment(Output i) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(i);
    float4 baseTexture = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv);
    #if defined(_CLIPPING)
        clip(baseTexture * IMPS(_BaseColor).a - IMPS(_Cutoff));
    #endif
    
    Surface surface;
    surface.color = baseTexture.rgb * IMPS(_BaseColor).rgb;
    surface.normal = normalize(i.normalWS);
    surface.alpha = baseTexture.a * IMPS(_BaseColor).a;

    float3 color = GetLighting(surface);
    return float4(color,surface.alpha);


}


#endif

