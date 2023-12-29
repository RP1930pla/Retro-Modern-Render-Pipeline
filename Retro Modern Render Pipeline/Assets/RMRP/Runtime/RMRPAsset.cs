using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/R-M Render Pipeline")]
public class RMRPAsset : RenderPipelineAsset
{
    [Header("Batching/Instancing Options:")]
    [SerializeField]
    bool useDynamicBatching = true;
    [SerializeField]
    bool useGPUInstancing = true;
    [SerializeField]
    bool useSRPBatcher = true;

    

    [SerializeField] [Header("Emulated lack of features:")]
    bool noFPU = false;
    [SerializeField]
    Vector2 VirtualResolution = new Vector2(320,240);
    [SerializeField]
    bool affineTextureMapping = true;
    protected override RenderPipeline CreatePipeline()
    {
        return new RMRenderPipeline(useDynamicBatching,useGPUInstancing,useSRPBatcher,noFPU,affineTextureMapping, VirtualResolution);
    }


}
