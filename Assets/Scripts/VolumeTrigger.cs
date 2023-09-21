using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VolumeTrigger : MonoBehaviour
{

    // private List<DogController> _dogs = new List<DogController>();
    private BoxCollider _boxCollider;

    [SerializeField] private GameObject _enableGameObject;
    [SerializeField] private UnityEvent _onDogEnter; 
    public event Action<DogController> onDogEnter; 
    // public List<DogController> dogs => _dogs;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Trigger");
        
        if(_enableGameObject)
            _enableGameObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        DogController dogController = other.GetComponentInParent<DogController>();
        if (dogController)
        {
            onDogEnter?.Invoke(dogController);
            _onDogEnter?.Invoke();
        }
        
        if(_enableGameObject)
            _enableGameObject.gameObject.SetActive(true);
    }
}
