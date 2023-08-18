using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BiteScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private Rigidbody _head;
    [SerializeField] private Transform _bitePoint;
    [SerializeField] private ConfigurableJoint _configurableJointReferences;

    private List<Collider> _collider = new List<Collider>();
    private List<Joint> _joints = new List<Joint>();
    private bool _isBiting;

    public bool isBiting
    {
        get => _isBiting;
    }


    public void DoBite()
    {
        _isBiting = true;
        List<Rigidbody> _rigidbodies = new List<Rigidbody>();
        foreach (Collider collider in _collider)
        {
            Rigidbody rigidbody = collider.GetComponentInParent < Rigidbody>();
            if (rigidbody)
            {
                if (_rigidbodies.Contains(rigidbody) == false)
                {
                    
                    _particles.gameObject.SetActive(false);
                    _rigidbodies.Add(rigidbody);
                    ConfigurableJoint joint = rigidbody.AddComponent<ConfigurableJoint>();
                    SpringController.CopySpring(joint, _configurableJointReferences);
                    _joints.Add(joint);
                    // joint.spring = 1000;
                    joint.autoConfigureConnectedAnchor = true;
                    // joint.anchor
                    Vector3 point = _head.transform.InverseTransformPoint(_bitePoint.transform.position);
                    Debug.Log(point);
                    // joint.connectedAnchor = point;

                    point = _head.transform.InverseTransformVector(_bitePoint.transform.position);
                    // point = _head.transform.InverseTransformDirection(_bitePoint.transform.position);
                    // Debug.Log(point);
                    // joint.damper = 2;
                    joint.connectedBody = _head;

                }
            }
        }
    }

    public void DoRelease()
    {
        _isBiting = false;

        foreach (Joint joint in _joints)
        {
            if (joint)
            {
                Destroy(joint);
            }
        } 
        
        _particles.gameObject.SetActive(true);

        _joints.Clear();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        _collider.Add(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        _collider.Remove(other);
    }
}
