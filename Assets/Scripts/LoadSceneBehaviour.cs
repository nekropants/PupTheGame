using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LoadSceneBehaviour : MonoBehaviour
{
    
    [SerializeField] private SceneReference _sceneToLoad;
    [SerializeField] private float _elapsedTimeSinceSceneStartRequired;
    private float _sceneStartTime = 0;

    public void Start()
    {
        _sceneStartTime = Time.time;
    }
    
    [ContextMenu("Do Load")]
    public void LoadScene()
    {
        if (Time.time - _sceneStartTime > _elapsedTimeSinceSceneStartRequired)
        {
            SceneManager.LoadScene(_sceneToLoad.SceneName);
        }
    }
}
