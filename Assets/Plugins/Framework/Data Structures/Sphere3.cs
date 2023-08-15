using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Sphere3
{
    public Vector3 Center { get { return _center; } set { _center = value; } }
    public float Radius { get { return _radius; } set { _radius = value; } }

    [SerializeField]
    private Vector3 _center;

    [SerializeField]
    private float _radius;

    public Sphere3(Vector3 center, float radius)
    {
        _radius = Math.Abs(radius);
        _center = center;
    }

    public Sphere3(SphereCollider sphereCollider)
    {
        _center = sphereCollider.transform.TransformPoint(sphereCollider.center);
        _radius = sphereCollider.radius * Mathf.Max(sphereCollider.transform.lossyScale.x, Mathf.Max(sphereCollider.transform.lossyScale.y, sphereCollider.transform.lossyScale.z));
    }


    public float DistanceTo(Vector3 point)
    {
        return Mathf.Max(0, (_center - point).magnitude - _radius);
    }

    public bool ContainsPoint(Vector3 point)
    {
        return (_center - point).sqrMagnitude <= _radius * _radius;
    }
}
