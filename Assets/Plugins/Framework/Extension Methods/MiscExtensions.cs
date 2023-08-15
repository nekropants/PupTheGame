using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Extension methods for miscellaneous types.
/// </summary>
public static class MiscExtensions
{
    public static Frustum GetFrustum(this Camera camera)
    {
        return new Frustum(camera);
    }

    public static Ray NormalizedScreenPointToRay(this Camera camera, Vector2 normalizedScreenPoint)
    {
        return camera.ScreenPointToRay(new Vector3(normalizedScreenPoint.x * camera.pixelWidth, normalizedScreenPoint.y * camera.pixelHeight, 0));
    }

    public static bool Raycast(this Plane plane, Ray ray, out Vector3 intersectionPoint)
    {
        float dist;
        intersectionPoint = Vector3.zero;

        if (plane.Raycast(ray, out dist))
        {
            intersectionPoint = ray.GetPoint(dist);
            return true;
        }
        return false;
    }

    public static bool ContainsPoint(this Collider collider, Vector3 point)
    {
        Type type = collider.GetType();

        if (type == typeof(BoxCollider)) return ((BoxCollider)collider).ContainsPoint(point);
        if (type == typeof(SphereCollider)) return ((SphereCollider)collider).ContainsPoint(point);
        if (type == typeof(CapsuleCollider)) return ((CapsuleCollider)collider).ContainsPoint(point);
        if (type == typeof(MeshCollider)) return ((MeshCollider)collider).ContainsPoint(point);

        throw new NotImplementedException();
    }

    public static bool ContainsPoint(this CapsuleCollider collider, Vector3 point)
    {
        throw new NotImplementedException(); // TODO: capsule3 class
    }

    public static bool ContainsPoint(this SphereCollider collider, Vector3 point)
    {
        return new Sphere3(collider).ContainsPoint(point);
    }


    public static bool ContainsPoint(this BoxCollider collider, Vector3 point)
    {
        return new Box3(collider).ContainsPoint(point);
    }


    public static bool ContainsPoint(this MeshCollider collider, Vector3 point)
    {
        if (collider.bounds.Contains(point))
        {
            Collider[] colliders = Physics.OverlapSphere(point, 1 << collider.gameObject.layer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == collider)
                {
                    return true;
                }
            }
        }

        return false;
    }


    public static void EnableEmission(this ParticleSystem particleSystem)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = true;
    }

    public static void DisableEmission(this ParticleSystem particleSystem)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = false;
    }

    public static void AddAlignmentTorque(this Rigidbody rigidbody, Vector3 targetDirection, float alignmentTorque, ForceMode forceMode)
    {
        Vector3 rotationAxis = Vector3.Cross(rigidbody.transform.forward, targetDirection.normalized);
        Vector3 angularVelocityChange = rotationAxis.normalized * Mathf.Asin(rotationAxis.magnitude);

        Quaternion worldSpaceTensor = rigidbody.transform.rotation * rigidbody.inertiaTensorRotation;
        Vector3 alignment = worldSpaceTensor * Vector3.Scale(rigidbody.inertiaTensor, Quaternion.Inverse(worldSpaceTensor) * angularVelocityChange);

        if (!alignment.HasNaNComponent())
        {
            rigidbody.AddTorque((alignment * alignmentTorque), forceMode);
        }
    }

    public static void AddAlignmentTorque(this Rigidbody rigidbody, Vector3 targetDirection, float alignmentTorque, float uprightingTorque, ForceMode forceMode)
    {
        Vector3 rotationAxis = Vector3.Cross(rigidbody.transform.forward, targetDirection.normalized);
        Vector3 angularVelocityChange = rotationAxis.normalized * Mathf.Asin(rotationAxis.magnitude);

        Quaternion worldSpaceTensor = rigidbody.transform.rotation * rigidbody.inertiaTensorRotation;
        Vector3 alignment = worldSpaceTensor * Vector3.Scale(rigidbody.inertiaTensor, Quaternion.Inverse(worldSpaceTensor) * angularVelocityChange);

        float uprightingAngle = rigidbody.transform.rotation.eulerAngles.z;
        uprightingAngle = uprightingAngle > 180 ? uprightingAngle - 360f : uprightingAngle;
        Vector3 uprighting = -rigidbody.transform.forward * (uprightingAngle / 90f);

        if (alignment.HasNaNComponent()) alignment = Vector3.zero;
        if (uprighting.HasNaNComponent()) uprighting = Vector3.zero;

        rigidbody.AddTorque((uprighting * uprightingTorque) + (alignment * alignmentTorque), forceMode);
    }

    public static void IgnoreCollisions(this IList<Collider> colliders, IList<Collider> otherColliders, bool ignore = true)
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            for (int j = 0; j < otherColliders.Count; j++)
            {
                Physics.IgnoreCollision(colliders[i], otherColliders[j], ignore);
            }
        }
    }

    public static void IgnoreCollisions(this IList<Collider> colliders, Collider otherCollider, bool ignore = true)
    {
        for (int j = 0; j < colliders.Count; j++)
        {
            Physics.IgnoreCollision(colliders[j], otherCollider, ignore);
        }
    }

    public static void IgnoreCollisions(this Collider collider, IList<Collider> otherColliders, bool ignore = true)
    {
        for (int j = 0; j < otherColliders.Count; j++)
        {
            Physics.IgnoreCollision(collider, otherColliders[j], ignore);
        }
    }

    public static bool LerpTowards(this Rigidbody rigidbody, Vector3 target, float speed, float maxDistance = Mathf.Infinity)
    {
        Vector3 toTarget = target - rigidbody.worldCenterOfMass;
        rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, toTarget * speed, maxDistance);

        return rigidbody.velocity.sqrMagnitude >= toTarget.sqrMagnitude;
    }

    public static bool LerpTowards(this Rigidbody rigidbody, Vector3 target, Vector3 worldPosition, float speed, float maxDistance = Mathf.Infinity)
    {
        Vector3 toTarget = target - worldPosition;
        rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, toTarget * speed, maxDistance);

        return rigidbody.velocity.sqrMagnitude >= toTarget.sqrMagnitude;
    }

    public static Vector2 GetDimensions(this TextMesh textMesh)
    {
        float currentLineWidth = 0;
        float currentLineheight = 0;
        float longestLineWidth = 0;
        float totalHeight = 0;
        int numLines = 1;
        CharacterInfo info;

        for (int i = 0; i < textMesh.text.Length; i++)
        {
            char c = textMesh.text[i];

            if (c == '\n')
            {
                numLines++;
                totalHeight += currentLineheight;
                currentLineheight = 0;
                currentLineWidth = 0;
                continue;
            }

            if (textMesh.font.GetCharacterInfo(c, out info, textMesh.fontSize, textMesh.fontStyle))
            {
                currentLineWidth += info.advance;
                if (currentLineWidth > longestLineWidth)
                {
                    longestLineWidth = currentLineWidth;
                }
                if (info.glyphHeight > currentLineheight)
                {
                    currentLineheight = info.glyphHeight;
                }
            }
        }
        totalHeight += currentLineheight;

        if (textMesh.lineSpacing < 1f)
        {
            return new Vector2(longestLineWidth, textMesh.font.lineHeight * ((numLines * textMesh.lineSpacing) + (1 - textMesh.lineSpacing))) * textMesh.characterSize * 0.1f;
        }

        //        return new Vector2(longestLineWidth, numLines * textMesh.font.lineHeight * textMesh.lineSpacing) * textMesh.characterSize * 0.1f;

        return new Vector2(longestLineWidth, totalHeight * textMesh.lineSpacing * 1.5f) * textMesh.characterSize * 0.1f;
    }

    public static Transform[] GetChildren(this Transform transform)
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = transform.GetChild(i);
        }

        return children;
    }


    public static T GetComponentInChildren<T>(this MonoBehaviour component, bool includeInactive) where T : Component
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(component.transform);
        while (queue.Count > 0)
        {
            Transform transform = queue.Dequeue();
            Component[] components = transform.GetComponents<Component>();

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T) return components[i] as T;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.gameObject.activeInHierarchy || includeInactive)
                {
                    queue.Enqueue(child);
                }
            }
        }

        return null;
    }


    /// <summary>
    /// Clears the current string of a string builder.
    /// </summary>
    public static void Clear(this StringBuilder stringBuilder)
    {
        stringBuilder.Length = 0;
    }

    public static Vector2 GetNormalizedPosition(this Rect rect, Vector2 position)
    {
        return new Vector2((position.x - rect.xMin) / rect.width, (position.y - rect.yMin) / rect.height);
    }

    public static bool ContainsCaseInsensitive(this string s, string substring)
    {
        return s.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    /// <summary>
    /// Checks whether this object extends or implements a type.
    /// </summary>
    /// <typeparam name="T">The type to check</typeparam>
    /// <returns>True if the type is asignable from this object's type</returns>
    public static bool IsTypeOf<T>(this object obj)
    {
        return typeof(T).IsAssignableFrom(obj.GetType());
    }

    /// <summary>
    /// Returns whether or not the object is between two comparable bounds. Inclusive on the lower bound, exclusive on the upper one.
    /// </summary>
    /// <param name="lower">The lower bound</param>
    /// <param name="upper">The upper bound</param>
    /// <returns>Whether or not the object is between the bounds</returns>
    public static bool IsBetween<T>(this T actual, T lower, T upper) where T : IComparable<T>
    {
        return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
    }

    /// <summary>
    /// Returns whether or not a string is either null or empty.
    /// </summary>
    /// <returns>Whether or not the string is null or empty</returns>
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static string GetPath(this GameObject gameObject)
    {
        if (gameObject == null)
        {
            return "NULL";
        }

#if UNITY_EDITOR
        if (UnityEditor.PrefabUtility.GetPrefabType(gameObject) == UnityEditor.PrefabType.Prefab)
        {
            return UnityEditor.AssetDatabase.GetAssetPath(gameObject);
        }
#endif

        Transform transform = gameObject.transform;
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }

        return gameObject.scene.name + "/" + path;
    }

    public static int NthIndexOf(this string s, int n, string match)
    {
        Assert.IsTrue(n > 0);

        int index = 0;
        for (int i = 0; i < n; i++)
        {
            if (index >= s.Length)
            {
                return -1;
            }

            index = s.IndexOf(match, index);
            if (index == -1)
            {
                return -1;
            }

            if (i == n - 1)
            {
                return index;
            }
        }

        return -1;
    }

    public static int NthIndexOf(this string s, int n, char c)
    {
        Assert.IsTrue(n > 0);

        int count = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == c)
            {
                count++;
                if (count == n)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    public static void InvokeRepeating(this MonoBehaviour mono, Action action, float delay, bool useUnscaledTime = false)
    {
        mono.StartCoroutine(InvokeRepeatingRoutine(action, delay, useUnscaledTime));
    }

    public static void InvokeRandomly(this MonoBehaviour mono, Action action, float minDelay, float maxDelay, bool useUnscaledTime = false)
    {
        mono.StartCoroutine(InvokeRandomlyRoutine(action, minDelay, maxDelay, useUnscaledTime));
    }

    public static void InvokeRandomly(this MonoBehaviour mono, Action action, FloatRange delayRange, bool useUnscaledTime = false)
    {
        mono.StartCoroutine(InvokeRandomlyRoutine(action, delayRange.Min, delayRange.Max, useUnscaledTime));
    }

    public static void InvokeDelayed(this MonoBehaviour mono, Action action, float delay, bool useUnscaledTime = false)
    {
        mono.StartCoroutine(InvokeDelayedRoutine(action, delay, useUnscaledTime));
    }

    static IEnumerator InvokeRepeatingRoutine(Action action, float delay, bool useUnscaledTime)
    {
        while (true)
        {
            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }

            action();
        }
    }

    static IEnumerator InvokeRandomlyRoutine(Action action, float minDelay, float maxDelay, bool useUnscaledTime)
    {
        while (true)
        {
            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(Random.Range(minDelay, maxDelay));
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            }

            action();
        }
    }

    /// <summary>
    /// Will execute imediately if delay is 0
    /// </summary>
    static IEnumerator InvokeDelayedRoutine(Action action, float delay, bool useUnscaledTime)
    {
        if (delay == 0)
        {
            action();
        }
        else
        {

            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }

            action();
        }
    }

    /// <summary>
    /// Returns the dimensions of the camera in world units.
    /// </summary>
    /// <returns>Worldspace camera dimensions</returns>
    public static Vector2 WorldspaceDimensions(this Camera camera)
    {
        float height = 2 * camera.orthographicSize;
        return new Vector2(height * camera.aspect, height);
    }

    public static Rect GetScreenspaceRect(this Camera camera, Vector3 cornerA, Vector3 cornerB)
    {
        cornerA = camera.WorldToScreenPoint(cornerA);
        cornerB = camera.WorldToScreenPoint(cornerB);

        return new Rect(Mathf.Min(cornerA.x, cornerB.x), Screen.height - Mathf.Max(cornerA.y, cornerB.y), Mathf.Abs(cornerA.x - cornerB.x), Mathf.Abs(cornerA.y - cornerB.y));
    }

    /// <summary>
    /// Returns the length of the curve in seconds by examining the last curve.
    /// </summary>
    /// <returns>length in secondes</returns>
    public static float LengthSeconds(this AnimationCurve curve)
    {
        // CHECK if there is NO CURVE
        if (curve.length == 0)
        {
            Debug.Log("NO CURVE ...");
            return 0;
        }

        Keyframe lastframe = curve[curve.length - 1];

        return lastframe.time;

    }

    /// <summary>
    /// Returns the value of the float squared.
    /// </summary>
    /// <returns>Value * value</returns>
    public static float Squared(this float f)
    {
        return f * f;
    }

    /// <summary>
    /// Checks whether this Type is a descendant of some other type.
    /// </summary>
    /// <param name="parentType">The base Type to check</param>
    /// <returns>True if this Type is assiagnable from the parentType</returns>
    public static bool IsTypeOf(this Type childType, Type parentType)
    {
        return parentType.IsAssignableFrom(childType);
    }

    /// <summary>
    /// Checks whether this Type is a descendant of some other type.
    /// </summary>
    /// <param name="T">The base Type to check</param>
    /// <returns>True if this Type is assiagnable from the parentType</returns>
    public static bool IsTypeOf<T>(this Type childType)
    {
        return typeof(T).IsAssignableFrom(childType);
    }

    /// <summary>
    /// Destroy the object immediately or delayed based on current editor playing state.
    /// </summary>
    public static void SmartDestroy(this UnityEngine.Object target)
    {
#if UNITY_EDITOR
        if (Application.isPlaying)
            UnityEngine.Object.Destroy(target);
        else
            UnityEngine.Object.DestroyImmediate(target);
#else
        UnityEngine.Object.Destroy (target);
#endif
    }

}
