#ifndef RETRO_FX_LIB
    #define RETRO_FX_LIB

    //INPUTS
    uint2 _Global_VirtualRes;

    float4 SnapToVirtualGrid(float4 posCS)
    {
        float4 snapToPixel = posCS;
        float4 vertex = snapToPixel;
        uint2 halfGrid = _Global_VirtualRes/2;

        vertex.xyz = snapToPixel.xyz / snapToPixel.w;
		vertex.x = floor(halfGrid.x * vertex.x) / halfGrid.x;
		vertex.y = floor(halfGrid.y * vertex.y) / halfGrid.y;
		vertex.xyz *= snapToPixel.w;

        return vertex;
    }

    #if defined (_AFFINE_TEXTURE)
        #define AFFINE_UV noperspective
    #else
        #define AFFINE_UV 
    #endif

    #if defined (_NO_FPU)
        #define SNAP_TO_PIXEL(posCS) posCS = SnapToVirtualGrid(posCS)
    #else
        #define SNAP_TO_PIXEL(posCS)
    #endif

#endif