using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegJoint : MonoBehaviour
{
    [SerializeField] private Transform _foot;
    [SerializeField] private ConfigurableJoint _joint;

    // Start is called before the first frame update
    void Start()
    {
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        _joint.targetPosition = -_joint.connectedBody.transform.InverseTransformPoint(_foot.transform.position);
        
    }
}
