using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinishLine : MonoBehaviour
{
    private List<DogController> _dogs = new List<DogController>();

    [SerializeField] private VolumeTrigger _volumeTrigger;
    [SerializeField] private DogConfiguration _configuration;

    private bool complete = false;
    private void Start()
    {
        _volumeTrigger.onDogEnter += OnDogCrossLine;
    }

    private void OnDogCrossLine(DogController obj)
    {
        if (_dogs.Contains(obj) == false)
            _dogs.Add(obj);

        if (complete == false)
        {
            Debug.Log("GameController.instance.dogs.Count  " +GameController.instance.dogs.Count );
            if (GameController.instance.dogs.Count == _dogs.Count || _dogs.Count > 2)
            {
                Invoke("LoadPodium" ,10f);
              
                complete = true;
            }
        }
    }

    private void LoadPodium()
    {
        if(_configuration.activeLevel.IsValid)
            SceneManager.UnloadSceneAsync(_configuration.activeLevel.SceneName);
         SceneManager.LoadScene("Scene_PrizeGiving");
    }
}
