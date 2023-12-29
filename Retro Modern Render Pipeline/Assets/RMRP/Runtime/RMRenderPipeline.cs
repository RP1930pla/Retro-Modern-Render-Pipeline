using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//called in tutorials CustomRenderPipeline : RenderPipeline
public class RMRenderPipeline : RenderPipeline
{
    //CAMERAS
    CameraRenderer renderer = new CameraRenderer();

    //INSTANCING OPTIONS
    bool useDynamicBatching,useGPUInstancing;
    public RMRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
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
