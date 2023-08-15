using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetResolution : MonoBehaviour {

    public int width = 720;
    public int heigth = 1280;

    public bool fullscreen = false;
    private void OnEnable()
    {
        Screen.SetResolution(width, heigth, fullscreen);
    }
}
