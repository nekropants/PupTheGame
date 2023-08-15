using System;
using UnityEngine;
using System.Collections;




/// <summary>
/// Extension methods for transforms.
/// </summary>
public static class TransformExtensions
{


    /// <summary>
    /// Increases the transform's X position.
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public static void AddX(this Transform transform, float amount)
    {
        transform.position = new Vector3(transform.position.x + amount, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Increases the transform's X position.
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public static void AddY(this Transform transform, float amount)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + amount, transform.position.z);
    }

    /// <summary>
    /// Increases the transform's X position.
    /// </summary>
    /// <param name="amount">The amount to add</param>
    public static void AddZ(this Transform transform, float amount)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + amount);
    }

    /// <summary>
    /// Sets the transform's global X position.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetX(this Transform transform, float value)
    {
        transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Sets the transform's global Y position.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetY(this Transform transform, float value)
    {
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
    }

    /// <summary>
    /// Sets the transform's global Z position.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetZ(this Transform transform, float value)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }

    /// <summary>
    /// Sets the transform's local X position.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetLocalX(this Transform transform, float value)
    {
        transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
    }

    /// <summary>
    /// Sets the transform's local Y position.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetLocalY(this Transform transform, float value)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
    }

    /// <summary>
    /// Sets the transform's local Z position.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetLocalZ(this Transform transform, float value)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
    }

    /// <summary>
    /// Sets the transform's local scale on the X axis.
    /// </summary>
    /// <param name="value">The new scale</param>
    public static void SetScaleX(this Transform transform, float value)
    {
        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Sets the transform's local scale on the Y axis.
    /// </summary>
    /// <param name="value">The new scale</param>
    public static void SetScaleY(this Transform transform, float value)
    {
        transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
    }

    /// <summary>
    /// Sets the transform's local scale on the Z axis.
    /// </summary>
    /// <param name="value">The new scale</param>
    public static void SetScaleZ(this Transform transform, float value)
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
    }

    /// <summary>
    /// Sets the transform's local scale on the all axes.
    /// </summary>
    /// <param name="value">The new position</param>
    public static void SetScale(this Transform transform, float value)
    {
        transform.localScale = new Vector3(value, value, value);
    }


    /// <summary>
    /// Resets the local transform to its default values.
    /// </summary>
    public static void Reset(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static Matrix4x4 GetTRSMatrix(this Transform transform)
    {
        return Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
    }

    public static Matrix4x4 GetLocalTRSMatrix(this Transform transform)
    {
        return Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
    }


}
