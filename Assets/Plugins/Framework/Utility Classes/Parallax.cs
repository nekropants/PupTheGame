using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {


    public bool followCamera = true;

    [ExposeIf("followCamera", false)]
    public Transform follow;

    public Vector3 parallax = new Vector3(1, 1, 0 );
    public Vector3 offsetM = new Vector3(1, 1, 0 );
    Vector3 start;
      Vector3 myStart;
      Vector3 initialOffset;


    private void Start()
    {

        if(followCamera)
        {
            follow = Camera.main.transform;
        }
        start = follow.transform.position;
        initialOffset = -(transform.position - follow.transform.position );

    }

  

    Vector3 offset;
	// Update is called once per frame
	void LateUpdate () {

        transform.position -= offset;

        Vector3 delta = follow.transform.position - start;


        offset = Vector3.Scale( initialOffset, offsetM);
        offset.x +=  delta.x* parallax.x;
        offset.y += delta.y * parallax.y;
        offset.z += delta.z * parallax.z;

        transform.position += offset;

    }
}
