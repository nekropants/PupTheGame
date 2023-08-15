using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryActiveRenderTexture : IDisposable
{
    private RenderTexture _originalRenderTexture;

    public TemporaryActiveRenderTexture(RenderTexture renderTexture)
    {
        _originalRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
    }

    public void Dispose()
    {
        RenderTexture.active = _originalRenderTexture;
    }
}
