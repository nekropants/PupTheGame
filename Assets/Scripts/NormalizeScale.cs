using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Transform parent = transform.parent;

        Quaternion parentRotation = transform.parent.rotation;
        transform.parent.rotation = Quaternion.identity;
        transform.localScale =  transform.parent.InverseTransformVector(Vector3.one);
        // transform.localScale = Vector3.one;
        transform.parent.rotation = parentRotation;

    }
}
