using UnityEngine;


/// <summary>
/// Extension methods for quaternions.
/// </summary>
public static class QuaternionExtensions
{

    public static Quaternion Reflect(this Quaternion rotation, Vector3 normal)
    {
        Vector3 targetForward = rotation * Vector3.forward;
        Vector3 targetUp = rotation * Vector3.up;

        Vector3 reflectedUp = Vector3.Reflect(targetUp, normal);
        Vector3 reflectedForward = Vector3.Reflect(targetForward, normal);

        return Quaternion.LookRotation(reflectedForward, reflectedUp);
    }

    public static bool IsValid(this Quaternion q)
    {
        if (float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w))
        {
            return false;
        }

        return Mathf.Approximately(Mathf.Sqrt((q.x * q.x) + (q.y * q.y) + (q.z * q.z) + (q.w * q.w)), 1f);
    }

    /// <summary>
    /// Calcuates the resulting quaternion when this quaternion is rotated by another.
    /// </summary>
    /// <param name="rotation">The quaternion rotation to apply</param>
    /// <returns>The rotated quaternion</returns>
    public static Quaternion AddRotation(this Quaternion q, Quaternion rotation)
    {
        return q * rotation;
    }

    /// <summary>
    /// Calcuates the resulting quaternion when this quaternion is unrotated by another.
    /// </summary>
    /// <param name="rotation">The quaternion rotation to inversly apply</param>
    /// <returns>The unrotated quaternion</returns>
    public static Quaternion SubtractRotation(this Quaternion q, Quaternion rotation)
    {
        return q * Quaternion.Inverse(rotation);
    }


    /// <summary>
    /// Calculate the quaternion's forward vector.
    /// </summary>
    /// <returns>The quaternion's forward vector</returns>
    public static Vector3 GetForwardVector(this Quaternion q)
    {
        return q * Vector3.forward;
    }

    /// <summary>
    /// Calculate the quaternion's up vector.
    /// </summary>
    /// <returns>The quaternion's up vector</returns>
    public static Vector3 GetUpVector(this Quaternion q)
    {
        return q * Vector3.up;
    }

    /// <summary>
    /// Calculate the quaternion's right vector.
    /// </summary>
    /// <returns>The quaternion's right vector</returns>
    public static Vector3 GetRightVector(this Quaternion q)
    {
        return q * Vector3.right;
    }

    /// <summary>
    /// Calculate the quaternion's back vector.
    /// </summary>
    /// <returns>The quaternion's back vector</returns>
    public static Vector3 GetBackVector(this Quaternion q)
    {
        return q * -Vector3.forward;
    }

    /// <summary>
    /// Calculate the quaternion's down vector.
    /// </summary>
    /// <returns>The quaternion's down vector</returns>
    public static Vector3 GetDownVector(this Quaternion q)
    {
        return q * -Vector3.up;
    }

    /// <summary>
    /// Calculate the quaternion's left vector.
    /// </summary>
    /// <returns>The quaternion's left vector</returns>
    public static Vector3 GetLeftVector(this Quaternion q)
    {
        return q * -Vector3.right;
    }
}
