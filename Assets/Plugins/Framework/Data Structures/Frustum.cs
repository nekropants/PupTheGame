using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Frustum
{
    public Plane FarPlane { get { return _farPlane; } }
    public Plane NearPlane { get { return _nearPlane; } }
    public Plane LeftPlane { get { return _leftPlane; } }
    public Plane RightPlane { get { return _rightPlane; } }
    public Plane TopPlane { get { return _topPlane; } }
    public Plane BottomPlane { get { return _bottomPlane; } }

    public Vector3 FarTopLeftCorner { get { return _farTopLeftCorner; } }
    public Vector3 FarTopRightCorner { get { return _farTopRightCorner; } }
    public Vector3 FarBottomLeftCorner { get { return _farBottomLeftCorner; } }
    public Vector3 FarBottomRightCorner { get { return _farTopLeftCorner; } }

    public Vector3 NearTopLeftCorner { get { return _nearTopLeftCorner; } }
    public Vector3 NearTopRightCorner { get { return _nearTopRightCorner; } }
    public Vector3 NearBottomLeftCorner { get { return _nearBottomLeftCorner; } }
    public Vector3 NearBottomRightCorner { get { return _nearBottomRightCorner; } }

    Plane _farPlane;
    Plane _nearPlane;
    Plane _leftPlane;
    Plane _rightPlane;
    Plane _topPlane;
    Plane _bottomPlane;

    private Vector3 _farTopLeftCorner;
    private Vector3 _farTopRightCorner;
    private Vector3 _farBottomLeftCorner;
    private Vector3 _farBottomRightCorner;

    private Vector3 _nearTopLeftCorner;
    private Vector3 _nearTopRightCorner;
    private Vector3 _nearBottomLeftCorner;
    private Vector3 _nearBottomRightCorner;

    public Frustum(Camera camera)
    {
        Vector3 nearCenter = camera.transform.position + camera.transform.forward * camera.nearClipPlane;
        Vector3 farCenter = camera.transform.position + camera.transform.forward * camera.farClipPlane;

        float halfNearHeight = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.nearClipPlane;
        float halfFarHeight = Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.farClipPlane;
        float halfNearWidth = halfNearHeight * camera.aspect;
        float halfFarWidth = halfFarHeight * camera.aspect;

        _farTopLeftCorner = farCenter + camera.transform.up * halfFarHeight - camera.transform.right * halfFarWidth;
        _farTopRightCorner = farCenter + camera.transform.up * halfFarHeight + camera.transform.right * halfFarWidth;
        _farBottomLeftCorner = farCenter - camera.transform.up * halfFarHeight - camera.transform.right * halfFarWidth;
        _farBottomRightCorner = farCenter - camera.transform.up * halfFarHeight + camera.transform.right * halfFarWidth;

        _nearTopLeftCorner = nearCenter + camera.transform.up * halfNearHeight - camera.transform.right * halfNearWidth;
        _nearTopRightCorner = nearCenter + camera.transform.up * halfNearHeight + camera.transform.right * halfNearWidth;
        _nearBottomLeftCorner = nearCenter - camera.transform.up * halfNearHeight - camera.transform.right * halfNearWidth;
        _nearBottomRightCorner = nearCenter - camera.transform.up * halfNearHeight + camera.transform.right * halfNearWidth;

        _farPlane = new Plane(_farTopLeftCorner, _farTopRightCorner, _farBottomRightCorner);
        _nearPlane = new Plane(_nearBottomRightCorner, _nearTopRightCorner, _nearTopLeftCorner);
        _leftPlane = new Plane(_farTopLeftCorner, _farBottomLeftCorner, _nearBottomLeftCorner);
        _rightPlane = new Plane(_farTopRightCorner, _nearTopRightCorner, _nearBottomRightCorner);
        _topPlane = new Plane(_farTopRightCorner, _farTopLeftCorner, _nearTopRightCorner);
        _bottomPlane = new Plane(_farBottomLeftCorner, _farBottomRightCorner, _nearBottomRightCorner);
    }

    public bool ContainsBounds(Bounds bounds, float tolerance = 0)
    {
        if (!ContainsPoint(bounds.min, tolerance)) return false;
        if (!ContainsPoint(bounds.max, tolerance)) return false;
        if (!ContainsPoint(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z), tolerance)) return false;
        if (!ContainsPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z), tolerance)) return false;
        if (!ContainsPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z), tolerance)) return false;
        if (!ContainsPoint(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z), tolerance)) return false;
        if (!ContainsPoint(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z), tolerance)) return false;
        if (!ContainsPoint(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z), tolerance)) return false;

        return true;
    }

    public bool ContainsPoint(Vector3 point, float tolerance = 0)
    {
        if (_leftPlane.GetDistanceToPoint(point) + tolerance < 0) return false;
        if (_rightPlane.GetDistanceToPoint(point) + tolerance < 0) return false;
        if (_topPlane.GetDistanceToPoint(point) + tolerance < 0) return false;
        if (_bottomPlane.GetDistanceToPoint(point) + tolerance < 0) return false;
        if (_nearPlane.GetDistanceToPoint(point) + tolerance < 0) return false;
        if (_farPlane.GetDistanceToPoint(point) + tolerance < 0) return false;

        return true;
    }

    public float GetSignedDistanceToPoint(Vector3 point)
    {
        float minDistance = _leftPlane.GetDistanceToPoint(point);

        minDistance = Mathf.Min(minDistance, _rightPlane.GetDistanceToPoint(point));
        minDistance = Mathf.Min(minDistance, _topPlane.GetDistanceToPoint(point));
        minDistance = Mathf.Min(minDistance, _bottomPlane.GetDistanceToPoint(point));
        minDistance = Mathf.Min(minDistance, _nearPlane.GetDistanceToPoint(point));
        minDistance = Mathf.Min(minDistance, _farPlane.GetDistanceToPoint(point));

        return -minDistance;
    }
}
