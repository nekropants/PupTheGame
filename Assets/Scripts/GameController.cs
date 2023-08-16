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
    private List<DogController> _dogs = new List<DogController>();
 
    
    public static GameController instance;

  
    private void Awake()
    {
        instance = this;
        
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
        _playerInputManager.onPlayerLeft += OnPlayerLeft;
    }
    private void OnPlayerLeft(PlayerInput obj)
    {
        _dogs.Remove( obj.gameObject.GetComponent<DogController>());
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        _dogs.Add( obj.gameObject.GetComponent<DogController>());


        SpawnVolume spawnVolume = FindObjectOfType<SpawnVolume>();

        if (spawnVolume)
        {
            obj.gameObject.transform.position = spawnVolume.transform.position;
        }
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
