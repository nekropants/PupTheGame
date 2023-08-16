using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpringController : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] ConfigurableJoint _spring;

    
    [SerializeField] SpringConfiguration _configuration;

    // Update is called once per frame
    void Update()
    {
        if (_configuration)
        {

            if (_rigidbody && _configuration.rigidbody)
            {
                _rigidbody.mass = _configuration.rigidbody.mass;
                _rigidbody.drag = _configuration.rigidbody.drag;
                _rigidbody.angularDrag = _configuration.rigidbody.angularDrag;
                _rigidbody.WakeUp();
            }

            if (_spring && _configuration.spring)
            {
                _spring.autoConfigureConnectedAnchor = _configuration.spring.autoConfigureConnectedAnchor;
                _spring.axis = _configuration.spring.axis;
                _spring.secondaryAxis = _configuration.spring.secondaryAxis;
                _spring.rotationDriveMode = _configuration.spring.rotationDriveMode;
                _spring.targetPosition = _configuration.spring.targetPosition;
                _spring.targetVelocity = _configuration.spring.targetVelocity;
                _spring.targetAngularVelocity = _configuration.spring.targetAngularVelocity;
                _spring.targetRotation = _configuration.spring.targetRotation;

                _spring.anchor = _configuration.spring.anchor;
                _spring.xDrive = _configuration.spring.xDrive;
                _spring.yDrive = _configuration.spring.yDrive;
                _spring.zDrive = _configuration.spring.zDrive;

                _spring.xMotion = _configuration.spring.xMotion;
                _spring.yMotion = _configuration.spring.yMotion;
                _spring.zMotion = _configuration.spring.zMotion;

                _spring.angularXDrive = _configuration.spring.angularXDrive;
                _spring.angularYZDrive = _configuration.spring.angularYZDrive;

                _spring.angularXMotion = _configuration.spring.angularXMotion;
                _spring.angularYMotion = _configuration.spring.angularYMotion;
                _spring.angularZMotion = _configuration.spring.angularZMotion;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Update();
    }
}
