using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    ScriptableRenderContext context;
    Camera camera;

    //Geometry Drawing indirect buffer
    const string cameraBufferName = "Render Camera Loop";
    CommandBuffer cameraBuffer = new CommandBuffer { name = cameraBufferName };

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        Setup();
        DrawVisibleGeometry();
        Submit();
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
        context.DrawSkybox(camera);
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
