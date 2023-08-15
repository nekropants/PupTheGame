using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    

    private MeshRenderer _renderer;
    [SerializeField]  private bool _snap;
    [SerializeField]  private float _snapIncrement = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        _renderer = GetComponent<MeshRenderer>();

        if (_snap)
        {
            _snap = false;

            // Snap();
        }
    }
    //
    // public void Snap()
    // {
    //     Bounds(MeshCollider);
    // }

}
