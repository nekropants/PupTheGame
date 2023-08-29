using System;
using System.Collections.Generic;
using UnityEngine;


public class StartingLight : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    private Color _onColor;
    private Color _offColor;
    void Start()
    {
        _onColor = _meshRenderer.sharedMaterial.GetColor("_BaseColor");
        _offColor = Color.Lerp(_meshRenderer.sharedMaterial.color, Color.grey, 0.5f);
    }

    public void SetOn()
    {
        _meshRenderer.enabled = true;
        // _meshRenderer.material.SetColor("_BaseColor", _onColor);
    }
    
    public void SetOff()
    {
        _meshRenderer.enabled = false;

        // _meshRenderer.material.SetColor("_BaseColor", _offColor);
    }
    
}
