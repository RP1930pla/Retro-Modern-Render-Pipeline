#ifndef UNLIT_PASS_INCLUDED
#define UNLIT_PASS_INCLUDED

struct Input
{
    float4 position : SV_POSITION;
    float4 uv : TEXCOORD0;
};

struct Output
{
    float4 posCS : SV_POSITION;
};

Output UnlitPassVertex(Input i)
{
    Output o;
    return o;
}

float4 UnlitPassFragment(Output o)
{
    return float4(1,1,1,1);
}


#endif

