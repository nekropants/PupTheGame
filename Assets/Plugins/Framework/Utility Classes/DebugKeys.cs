using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugKeys : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.F2))
        {
            if(Time.time ==1)
            {
              
                    Time.timeScale = 4;
            }
            else
            {
                Time.timeScale = 1;
            }
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.LeftControl))
                Time.timeScale = .1f;
else
            Time.timeScale = 10;
        }
        else 
        {
            Time.timeScale = 1;

        }


    }
}
