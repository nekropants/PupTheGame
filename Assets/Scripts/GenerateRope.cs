using System.Collections;
using System.Collections.Generic;
using RichardPieterse;
using Unity.Mathematics;
using UnityEngine;


public class GenerateRope : MonoBehaviour
{
    [SerializeField] private GenerateRopeConfiguration _configuration;
    [SerializeField] private int length = 5;
    [SerializeField] private bool _addJointToStart;
    

    void Start()
    {
        AvoidCollision();
    }
    
    private void AvoidCollision()
    {      
        for (int i = 2; i < length; i++)
        {
            Collider colliderA = transform.GetChild(i-2).GetComponent<Collider>();
            Collider colliderB = transform.GetChild(i).GetComponent<Collider>();
            Physics.IgnoreCollision(colliderA, colliderB);
        }
    }


    [ContextMenu("Generate")]
    private void Generate()
    {
        transform.DestroyChildren();
        for (int i = 0; i < length; i++)
        {
            GameObject instantiate = Instantiate(_configuration.segment.gameObject,  transform );
            instantiate.transform.localPosition = Vector3.forward * i * 0.5f;
            instantiate.transform.localRotation = quaternion.identity;
            instantiate.transform.localScale = Vector3.one;
            instantiate.AddComponent<Rigidbody>();
            instantiate.name = "Segment" + i;
        }

        for (int i = 1; i < length; i++)
        {
            ConfigurableJoint configurableJoint = transform.GetChild(i).gameObject.AddComponent<ConfigurableJoint>();
            configurableJoint.autoConfigureConnectedAnchor = false;
            configurableJoint.connectedBody =   transform.GetChild(i-1).GetComponent<Rigidbody>();
            configurableJoint.connectedAnchor = new Vector3(0, 0, 0.5f);
            configurableJoint.anchor = new Vector3(0, 0, 0);
        }

        if (_addJointToStart)
        {
            ConfigurableJoint configurableJoint = transform.GetChild(0).gameObject.AddComponent<ConfigurableJoint>();
            configurableJoint.autoConfigureConnectedAnchor = true;
        }
  
        for (int i = 0; i < length; i++)
        {
            ApplyRopeConfiguration(transform.GetChild(i).gameObject);
        }
    }

    private void ApplyRopeConfiguration(GameObject segment)
    {
        Rigidbody rigidbody = segment.GetComponent<Rigidbody>();
        ConfigurableJoint configurableJoint = segment.GetComponent<ConfigurableJoint>();

        if (rigidbody)
        {
            rigidbody.mass = _configuration.segmentMass;
            rigidbody.drag = _configuration.segmentDrag;
        }

        if (configurableJoint)
        {
            configurableJoint.xMotion = ConfigurableJointMotion.Locked;
            configurableJoint.yMotion = ConfigurableJointMotion.Locked;
            configurableJoint.zMotion = ConfigurableJointMotion.Locked;
            
            configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
            configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;


            JointDrive angularXDrive = configurableJoint.angularXDrive;
            angularXDrive.positionSpring = _configuration.springStrength;
            angularXDrive.positionDamper = _configuration.springDamper;
            configurableJoint.angularXDrive = angularXDrive;
            
            JointDrive angularYZDrive = configurableJoint.angularYZDrive;
            angularYZDrive.positionSpring = _configuration.springStrength;
            angularYZDrive.positionDamper = _configuration.springDamper;
            configurableJoint.angularYZDrive = angularYZDrive;
        }
    }
}
