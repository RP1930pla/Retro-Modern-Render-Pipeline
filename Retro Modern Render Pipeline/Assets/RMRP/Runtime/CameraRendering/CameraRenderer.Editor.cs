using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

partial class CameraRenderer
{
    partial void DrawUnsuportedShaders();

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
    #endif

}
