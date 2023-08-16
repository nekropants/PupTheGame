using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnVolume : MonoBehaviour
{
    private void Awake()
    {
        // GameController.CreateGameInstanceIfNeeded();
        
        Scene scene = SceneManager.GetSceneByName("Scene_System");
        
        // Debug.Log(scene);
        if(scene.IsValid() == false)
            SceneManager.LoadScene("Scene_System", LoadSceneMode.Additive);
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan.WithAlpha(0.3f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
