using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeTrigger : MonoBehaviour
{
    private BoxCollider _boxCollider;

    [SerializeField] private GameObject _enableGameObject;
    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = gameObject.AddComponent<BoxCollider>();
        _boxCollider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Trigger");
    }

    private void OnTriggerEnter(Collider other)
    {
        _enableGameObject.gameObject.SetActive(true);
    }
}
