using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBasedOnMaterial : MonoBehaviour
{
    private MeshRenderer _renderer;

    private void OnDrawGizmosSelected()
    {
        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();

        
        gameObject.name = _renderer.sharedMaterial.name;
    }
}
