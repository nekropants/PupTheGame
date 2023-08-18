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
        if(Application.isPlaying == false)
            FixedUpdate();
    }
    void FixedUpdate()
    {
        if(enabled == false)
            return;
        
        if (_configuration)
        {

            if (_rigidbody && _configuration.rigidbody)
            {
                _rigidbody.mass = _configuration.rigidbody.mass;
                _rigidbody.drag = _configuration.rigidbody.drag;
                _rigidbody.angularDrag = _configuration.rigidbody.angularDrag;
                _rigidbody.WakeUp();
            }


            if (_spring && _configuration)
            {
                ConfigurableJoint toSpring = _spring;
                ConfigurableJoint fromSpring = _configuration.spring;
                
                CopySpring(toSpring, fromSpring);
            }
        }
    }

    public static void CopySpring(ConfigurableJoint toSpring, ConfigurableJoint fromSpring)
    {
        // toSpring.autoConfigureConnectedAnchor = fromSpring.autoConfigureConnectedAnchor;
        // toSpring.axis = fromSpring.axis;
        // toSpring.secondaryAxis = fromSpring.secondaryAxis;
        toSpring.connectedAnchor = fromSpring.connectedAnchor;
        toSpring.rotationDriveMode = fromSpring.rotationDriveMode;
        toSpring.targetPosition = fromSpring.targetPosition;
        toSpring.targetVelocity = fromSpring.targetVelocity;
        toSpring.targetAngularVelocity = fromSpring.targetAngularVelocity;
        toSpring.targetRotation = fromSpring.targetRotation;
        //
        toSpring.anchor = fromSpring.anchor;
        toSpring.xDrive = fromSpring.xDrive;
        toSpring.yDrive = fromSpring.yDrive;
        toSpring.zDrive = fromSpring.zDrive;

        toSpring.xMotion = fromSpring.xMotion;
        toSpring.yMotion = fromSpring.yMotion;
        toSpring.zMotion = fromSpring.zMotion;
        
        toSpring.angularXDrive = fromSpring.angularXDrive;
        toSpring.angularYZDrive = fromSpring.angularYZDrive;
        
        toSpring.angularXMotion = fromSpring.angularXMotion;
        toSpring.angularYMotion = fromSpring.angularYMotion;
        toSpring.angularZMotion = fromSpring.angularZMotion;
        
        
        toSpring.linearLimit = fromSpring.linearLimit;
        toSpring.linearLimitSpring = fromSpring.linearLimitSpring;
        toSpring.lowAngularXLimit = fromSpring.lowAngularXLimit;
        toSpring.highAngularXLimit = fromSpring.highAngularXLimit;
        toSpring.angularYZLimitSpring = fromSpring.angularYZLimitSpring;
        toSpring.angularYLimit = fromSpring.angularYLimit;
        toSpring.angularXLimitSpring = fromSpring.angularXLimitSpring;
    }

    private void OnDrawGizmosSelected()
    {
        // Update();
    }

    public void SetConfig(SpringConfiguration springConfig)
    {
        _configuration = springConfig;
        FixedUpdate();
    }
}
