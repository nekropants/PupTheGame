using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Box3 : ISerializationCallbackReceiver
{
    public Matrix4x4 AsMatrix { get { return Matrix4x4.TRS(_center, _rotation, _dimensions * 0.5f); } }
    public Vector3 Center { get { return _center; } set { _center = value; } }
    public Quaternion Rotation { get { return _rotation; } set { _rotation = value; } }
    public Vector3 Dimensions { get { return _dimensions; } set { _dimensions = value; } }
    public Vector3 Extents { get { return _dimensions * 0.5f; } set { _dimensions = value * 2f; } }
    public float Volume { get { return _dimensions.x * _dimensions.y * _dimensions.z; } }

    public Vector3 ForwardFacePosition { get { return _center + _rotation * new Vector3(0, 0, _dimensions.z * 0.5f); } }
    public Vector3 BackFacePosition { get { return _center + _rotation * new Vector3(0, 0, _dimensions.z * -0.5f); } }
    public Vector3 LeftFacePosition { get { return _center + _rotation * new Vector3(_dimensions.x * -0.5f, 0, 0); } }
    public Vector3 RightFacePosition { get { return _center + _rotation * new Vector3(_dimensions.x * 0.5f, 0, 0); } }
    public Vector3 TopFacePosition { get { return _center + _rotation * new Vector3(0, _dimensions.y * 0.5f, 0); } }
    public Vector3 BottomFacePosition { get { return _center + _rotation * new Vector3(0, _dimensions.y * -0.5f, 0); } }

    public Vector3 ForwardDirection { get { return Rotation.GetForwardVector(); } }
    public Vector3 BackDirection { get { return Rotation.GetBackVector(); } }
    public Vector3 LeftDirection { get { return Rotation.GetLeftVector(); } }
    public Vector3 RightDirection { get { return Rotation.GetRightVector(); } }
    public Vector3 UpDirection { get { return Rotation.GetUpVector(); } }
    public Vector3 DownDirection { get { return Rotation.GetDownVector(); } }

    [SerializeField]
    private Vector3 _center;

    [SerializeField]
    private Quaternion _rotation;

    [SerializeField]
    private Vector3 _dimensions;

    public Box3(Vector3 center, Quaternion rotation, Vector3 dimensions)
    {
        _center = center;
        _rotation = rotation;
        _dimensions = dimensions;
    }

    public Box3(Vector3 center, Vector3 dimensions)
    {
        _center = center;
        _rotation = Quaternion.identity;
        _dimensions = dimensions;
    }

    public Box3(Matrix4x4 matrix)
    {
        _center = matrix.GetTranslation();
        _rotation = matrix.GetRotation();
        _dimensions = matrix.GetScale() * 2f;
    }

    public Box3(Transform transform)
    {
        _center = transform.position;
        _rotation = transform.rotation;
        _dimensions = transform.lossyScale;
    }

    public Box3(Bounds bounds)
    {
        _center = bounds.center;
        _rotation = Quaternion.identity;
        _dimensions = bounds.extents * 2f;
    }

    public Box3(BoxCollider boxCollider)
    {
        _center = boxCollider.transform.TransformPoint(boxCollider.center);
        _rotation = boxCollider.transform.rotation;
        _dimensions = boxCollider.transform.lossyScale.MultiplyComponentWise(boxCollider.size);
    }

    public Vector3[] GetCorners()
    {
        Vector3[] corners = new Vector3[8];
        Matrix4x4 matrix = Matrix4x4.TRS(_center, _rotation, _dimensions * 0.5f);

        corners[0] = matrix.MultiplyPoint3x4(new Vector3(-1, -1, 1));
        corners[1] = matrix.MultiplyPoint3x4(new Vector3(1, -1, 1));
        corners[2] = matrix.MultiplyPoint3x4(new Vector3(1, -1, -1));
        corners[3] = matrix.MultiplyPoint3x4(new Vector3(-1, -1, -1));
        corners[4] = matrix.MultiplyPoint3x4(new Vector3(-1, 1, 1));
        corners[5] = matrix.MultiplyPoint3x4(new Vector3(1, 1, 1));
        corners[6] = matrix.MultiplyPoint3x4(new Vector3(1, 1, -1));
        corners[7] = matrix.MultiplyPoint3x4(new Vector3(-1, 1, -1));

        return corners;
    }

    public Vector3 TransformPoint(Vector3 localPoint)
    {
        return Matrix4x4.TRS(_center, _rotation, _dimensions * 0.5f).MultiplyPoint3x4(localPoint);
    }

    public Vector3 InverseTransformPoint(Vector3 point)
    {
        return Matrix4x4.TRS(_center, _rotation, _dimensions * 0.5f).inverse.MultiplyPoint3x4(point);
    }

    public bool ContainsPoint(Vector3 point)
    {
        point = Matrix4x4.TRS(_center, _rotation, _dimensions * 0.5f).inverse.MultiplyPoint3x4(point);
        return point.x >= -1f && point.x <= 1f && point.y >= -1f && point.y <= 1f && point.z >= -1f && point.z <= 1f;
    }



    public Vector3 GetClosestPositionXZ(Vector3 fromPosition)
    {


        float minDist = Mathf.Infinity;
        Vector3 closestEdge = Vector3.zero;

        for (int i = 0; i < 4; i++)
        {
            Vector3 edge = Vector3.zero;

            if (i == 0) edge = Center + new Vector3(Extents.x, 0, 0);
            if (i == 1) edge = Center + new Vector3(-Extents.x, 0, 0);
            if (i == 2) edge = Center + new Vector3(0, 0, -Extents.z);
            if (i == 3) edge = Center + new Vector3(0, 0, Extents.z);

            float dist = fromPosition.To(edge).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closestEdge = edge;
            }
        }

        Gizmos.color = Color.white;
        return closestEdge;
    }

    public void OnBeforeSerialize()
    {
        if (!_rotation.IsValid())
        {
            if (_dimensions == Vector3.zero)
            {
                _dimensions = Vector3.one;
            }

            _rotation = Quaternion.identity;
        }
    }

    public void OnAfterDeserialize()
    {

    }

    //public static Box3 Create(Vector3 corner1, Vector3 corner2)
    //{
    //    Box3 box3 = new Box3();
    //    box3.Center = (corner1 + corner2)/2;
    //    box3.Dimensions = (corner1 - corner2).Abs();

    //    return box3;
    //}

    //public void  Encapsulate(params Vector3[] points)
    //{
    //    foreach (Vector3 point in points)
    //    {

    //    }
    //}
}
