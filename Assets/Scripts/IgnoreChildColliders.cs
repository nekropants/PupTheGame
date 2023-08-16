using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreChildColliders : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
        foreach (Collider colliderA in componentsInChildren)
        {
            foreach (Collider colliderB in componentsInChildren)
            {
                Physics.IgnoreCollision(colliderA, colliderB, true);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
