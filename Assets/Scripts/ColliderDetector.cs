using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    
    private List<Collider> _colliders = new List<Collider>();


    public bool hasColliders => _colliders.Count >0;
    private void OnTriggerEnter(Collider other)
    {
        // if (_layerMask.Contains(other.gameObject.layer))
        {
            _colliders.Add(other);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_colliders.Contains(other))
        {
            _colliders.Remove(other);
        }
    }
}