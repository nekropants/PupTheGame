using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yBasedOnZ : MonoBehaviour {

    public float m = 1;
	void OnDrawGizmos()
    {
        if (enabled)

            transform.SetY(transform.position.z * m* yBasedOnZController.M);

    }

    private void LateUpdate()
    {
        if (enabled)

            transform.SetY(transform.position.z * m* yBasedOnZController.M);
    }
}