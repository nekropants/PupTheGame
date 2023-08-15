using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {


    public static bool left;
    public static bool right;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        left = right = false;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            left = pos.x < .5f;
            right = pos.x > .5f;

        }
    }
}
