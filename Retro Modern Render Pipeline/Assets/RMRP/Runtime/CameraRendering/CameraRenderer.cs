using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");
    #endregion

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        //Cull Object first
        if (!CullObjects())
        {
            return;
        }

        //Setup camera properties
        Setup();

        //Draw commands
        DrawVisibleGeometry();
        DrawUnsuportedShaders();
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

        //Clean render buffer before rendering again
        cameraBuffer.ClearRenderTarget(true, true, Color.clear);

        //Show command buffer loop on Frame Debugger
        cameraBuffer.BeginSample(cameraBufferName);

        ExecuteBuffer();
    }


    void DrawVisibleGeometry()
    {
        //Setup everything for drawing opaque and skybox objects
        //Sorting info based on camera
        var sortingSettings = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque};
        //Drawings settings of each renderer in the scene that is not culled
        var drawingSettings = new DrawingSettings(unlitShaderTagId,sortingSettings);
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
        cameraBuffer.EndSample(cameraBufferName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(cameraBuffer);
        cameraBuffer.Clear();
    }
}