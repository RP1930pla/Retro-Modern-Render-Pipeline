using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/R-M Render Pipeline")]
public class RMRPAsset : RenderPipelineAsset
{
    [SerializeField]
    bool useDynamicBatching = true, useGPUInstancing = true, useSRPBatcher = true;
    protected override RenderPipeline CreatePipeline()
    {
        return new RMRenderPipeline(useDynamicBatching,useGPUInstancing,useSRPBatcher);
    }


}
