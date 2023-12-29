using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//called in tutorials CustomRenderPipeline : RenderPipeline
public class RMRenderPipeline : RenderPipeline
{
    //CAMERAS
    CameraRenderer renderer = new CameraRenderer();

    //CACHED GLOBAL KEYWORDS & VARIABLES
    GlobalKeyword FPU = GlobalKeyword.Create("_NO_FPU");
    GlobalKeyword AFFINE_TEXTURE = GlobalKeyword.Create("_AFFINE_TEXTURE");
    int _Global_VirtualRes = Shader.PropertyToID(nameof(_Global_VirtualRes));
    //INSTANCING OPTIONS
    bool useDynamicBatching,useGPUInstancing;

    public RMRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher, bool noFPU, bool affineTexture, Vector2 virtualRes)
    {
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;

        //Set Retro Defect Keywords
        Shader.SetKeyword(FPU, noFPU);
        Shader.SetKeyword(AFFINE_TEXTURE, affineTexture);
        Shader.SetGlobalVector(_Global_VirtualRes, virtualRes);

    }
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
    }

    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            renderer.Render(context, cameras[i], useDynamicBatching,useGPUInstancing);
        }
    }
}
