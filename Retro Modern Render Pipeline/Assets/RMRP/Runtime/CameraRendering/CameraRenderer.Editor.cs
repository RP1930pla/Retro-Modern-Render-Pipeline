using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

partial class CameraRenderer
{
    partial void DrawUnsuportedShaders();
    partial void DrawGizmos();
    partial void PrepareUIForSceneWindow();
    partial void PrepareBuffer();

    #if UNITY_EDITOR
    static Material errorMaterial;
    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };


    partial void DrawUnsuportedShaders()
    {
        if (errorMaterial == null)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera)) { overrideMaterial = errorMaterial};
        var filteringSettings = FilteringSettings.defaultValue;

        for (int i = 1; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        
        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    partial void PrepareUIForSceneWindow()
    {
        if (camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }


    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareBuffer()
    {
        Profiler.BeginSample("Editor Only");
        cameraBuffer.name = SampleName = camera.name;
        Profiler.EndSample();
    }

#endif

}
