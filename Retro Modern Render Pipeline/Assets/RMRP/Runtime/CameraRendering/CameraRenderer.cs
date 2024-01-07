using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    ScriptableRenderContext context;
    Camera camera;

    //Geometry Drawing indirect buffer
    const string cameraBufferName = "Render Camera Loop";
    CommandBuffer cameraBuffer = new CommandBuffer { name = cameraBufferName };

    //Struct containing the results of a culling operation
    CullingResults cullingResults;

    #region Shader Tags
    //Shader Tags used for rendering
    public static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit"),
        litShaderTagId = new ShaderTagId("CustomLit");
    #endregion

    #if UNITY_EDITOR
    string SampleName { get; set; }
    #else
    string SampleName => cameraBufferName;
    #endif

    public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
    {
        this.context = context;
        this.camera = camera;

        //Draw UI in scene view
        PrepareBuffer();
        PrepareUIForSceneWindow();

        //Cull Object first
        if (!CullObjects())
        {
            return;
        }

        //Setup camera properties
        Setup();

        //Draw commands
        DrawVisibleGeometry(useDynamicBatching,useGPUInstancing);
        DrawUnsuportedShaders();
        DrawGizmos();
        Submit();
    }



    bool CullObjects()
    {
        if(camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }



    //Pushes to GPU Matrices used to transform geometry, like the View Projection Matrix
    void Setup()
    {
        context.SetupCameraProperties(camera);
        CameraClearFlags flagsToClear = camera.clearFlags;
        //Clean render buffer before rendering again
        cameraBuffer.ClearRenderTarget(flagsToClear <= CameraClearFlags.Depth, flagsToClear <= CameraClearFlags.Color, flagsToClear == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);
        //Show command buffer loop on Frame Debugger
        cameraBuffer.BeginSample(SampleName);

        ExecuteBuffer();
    }


    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        //Setup everything for drawing opaque and skybox objects
        //Sorting info based on camera
        var sortingSettings = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque};
        //Drawings settings of each renderer in the scene that is not culled
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings)
        {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing
        };

        //Add here all passes
        drawingSettings.SetShaderPassName(1, litShaderTagId);

        //Render layers of each renderer
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
        context.DrawSkybox(camera);


        //Change settings to render transparent queue
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);


    }

    //Submits all rendering commands to the rendering loop to execute render commands
    //Always last to be called
    void Submit()
    {
        cameraBuffer.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(cameraBuffer);
        cameraBuffer.Clear();
    }
}
