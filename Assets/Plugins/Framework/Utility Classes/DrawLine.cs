using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    bool ray = false;


    public Color color = Color.white;
    public Vector3 direction = Vector3.up;
    private void OnDrawGizmos()
    {

        Gizmos.color = color;
        Gizmos.DrawRay(transform.position, direction);

        
    }

}
