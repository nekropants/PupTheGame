using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private AudioSource _audioSource;
    private List<DogController> _dogs = new List<DogController>();
    
    public static GameController instance;

    public List<DogController> dogs => _dogs;


    private void Awake()
    {
        instance = this;
        
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
        _playerInputManager.onPlayerLeft += OnPlayerLeft;
    }
    private void OnPlayerLeft(PlayerInput obj)
    {
        dogs.Remove( obj.gameObject.GetComponent<DogController>());
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        var dogController = obj.gameObject.GetComponent<DogController>();
        dogs.Add( dogController);
        
        _audioSource.Play();


        SpawnVolume spawnVolume = FindObjectOfType<SpawnVolume>();

        if (spawnVolume)
        {
            obj.gameObject.transform.position = spawnVolume.transform.position;
        }
        
        if( PlayerColorController.Instance)
            PlayerColorController.Instance.AssignRandomColor(dogController);
    }


    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
