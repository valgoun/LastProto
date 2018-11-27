using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


[Serializable]
[PostProcess(typeof(FogOfWarRenderer), PostProcessEvent.AfterStack, "PostProcess/FogOfWar", true)]
public class FogOfWar : PostProcessEffectSettings
{
    public ColorParameter FogColor = new ColorParameter { value = Color.white };
    [Range(0f, 1f)]
    public FloatParameter Density = new FloatParameter { value = 1.0f };
}

public sealed class FogOfWarRenderer : PostProcessEffectRenderer<FogOfWar>
{

    private FogManager _fogManager;

    public override void Render(PostProcessRenderContext context)
    {
        _fogManager = _fogManager ?? FogManager.Instance;

        if (_fogManager == null || _fogManager.VisionElements.Count == 0)
            return;

        var dataArray = _fogManager.VisionElements.Select(x => new Vector4(x.Position.x, x.Position.y, x.Position.z, x.VisionRange)).ToArray();
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/FogOfWar"));

        sheet.properties.SetFloat("_density", settings.Density);
        sheet.properties.SetColor("_fogColor", settings.FogColor);
        sheet.properties.SetVectorArray("_VisionPoints", dataArray);
        sheet.properties.SetInt("_VisionUnit", dataArray.Length);
        sheet.properties.SetMatrix("_inverseView", context.camera.cameraToWorldMatrix);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
