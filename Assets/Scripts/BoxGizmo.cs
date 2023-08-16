using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxGizmo : MonoBehaviour
{
    private void Start()
    {
    }

    [SerializeField] private Color _solidColor = Color.white; 
    [SerializeField] private Color _outlineColor = Color.white; 
    private void OnDrawGizmos()
    {
        if(enabled == false)
            return;
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = _solidColor;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        
        Gizmos.color = _outlineColor;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
