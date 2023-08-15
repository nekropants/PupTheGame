using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yBasedOnZController : SingletonBehaviour<yBasedOnZController> {

    public float m = 1;

    public static  float M
    {
        get
        {
            if (Instance)
                return Instance.m;

            return 1;
        }
    }




}
