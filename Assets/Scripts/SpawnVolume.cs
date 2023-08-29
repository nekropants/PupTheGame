using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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

    public Vector3 GetRandomSpawnPosition()
    {
        Vector3 position =
            transform.TransformPoint(new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f));
        position.y = transform.position.y;
        return position;
    }
}
