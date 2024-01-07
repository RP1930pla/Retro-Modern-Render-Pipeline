#ifndef STANDARD_SURFACE_INCLUDED
#define STANDARD_SURFACE_INCLUDED

///Type that defines the shading properties
struct Surface 
{
    float3 normal;
    float3 color;
    float alpha;
};

#endif