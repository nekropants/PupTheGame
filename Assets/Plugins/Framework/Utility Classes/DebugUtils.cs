using System;
using System.Text;
using UnityEngine;
using System.Collections;

/// <summary>
/// Utility class to provide extra debug functionality.
/// </summary>
public static class DebugUtils
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end, Color colour)
    {
        if (Application.isPlaying)
        {
            Debug.DrawLine(start, end, colour);
        }
        else
        {
            Gizmos.color = colour;
            Gizmos.DrawLine(start, end);
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        DrawLine(start, end, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSphere(Vector3 center, float radius, Color colour)
    {
#if UNITY_EDITOR
        Gizmos.color = colour;
        Gizmos.DrawWireSphere(center, radius);
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSphere(Vector3 center, float radius)
    {
        DrawSphere(center, radius, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSphere(Sphere3 sphere)
    {
        DrawSphere(sphere.Center, sphere.Radius, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSphere(Sphere3 sphere, Color colour)
    {
        DrawSphere(sphere.Center, sphere.Radius, colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSolidSphere(Vector3 center, float radius, Color colour)
    {
#if UNITY_EDITOR
        Gizmos.color = colour;
        Gizmos.DrawSphere(center, radius);
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSolidSphere(Vector3 center, float radius)
    {
        DrawSphere(center, radius, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSolidSphere(Sphere3 sphere)
    {
        DrawSphere(sphere.Center, sphere.Radius, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSolidSphere(Sphere3 sphere, Color colour)
    {
        DrawSphere(sphere.Center, sphere.Radius, colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawBox(Box3 box, Color colour)
    {
#if UNITY_EDITOR
        Vector3[] corners = box.GetCorners();

        // Bottom
        DrawLine(corners[0], corners[1], colour);
        DrawLine(corners[1], corners[2], colour);
        DrawLine(corners[2], corners[3], colour);
        DrawLine(corners[3], corners[0], colour);

        // Top
        DrawLine(corners[4], corners[5], colour);
        DrawLine(corners[5], corners[6], colour);
        DrawLine(corners[6], corners[7], colour);
        DrawLine(corners[7], corners[4], colour);

        // Uprights
        DrawLine(corners[0], corners[4], colour);
        DrawLine(corners[1], corners[5], colour);
        DrawLine(corners[2], corners[6], colour);
        DrawLine(corners[3], corners[7], colour);
#endif
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawBox(Vector3 center, Vector3 dimensions, Quaternion rotation, Color colour)
    {
#if UNITY_EDITOR
        Vector3 extents = dimensions * 0.5f;

        Vector3 frontTopLeft = rotation * new Vector3(-extents.x, extents.y, -extents.z) + center;
        Vector3 frontTopRight = rotation * new Vector3(extents.x, extents.y, -extents.z) + center;
        Vector3 frontBottomLeft = rotation * new Vector3(-extents.x, -extents.y, -extents.z) + center;
        Vector3 frontBottomRight = rotation * new Vector3(extents.x, -extents.y, -extents.z) + center;
        Vector3 backTopLeft = rotation * new Vector3(-extents.x, extents.y, extents.z) + center;
        Vector3 backTopRight = rotation * new Vector3(extents.x, extents.y, extents.z) + center;
        Vector3 backBottomLeft = rotation * new Vector3(-extents.x, -extents.y, extents.z) + center;
        Vector3 backBottomRight = rotation * new Vector3(extents.x, -extents.y, extents.z) + center;

        DrawLine(frontTopLeft, frontTopRight, colour);
        DrawLine(frontTopRight, frontBottomRight, colour);
        DrawLine(frontBottomRight, frontBottomLeft, colour);
        DrawLine(frontBottomLeft, frontTopLeft, colour);

        DrawLine(backTopLeft, backTopRight, colour);
        DrawLine(backTopRight, backBottomRight, colour);
        DrawLine(backBottomRight, backBottomLeft, colour);
        DrawLine(backBottomLeft, backTopLeft, colour);

        DrawLine(frontTopLeft, backTopLeft, colour);
        DrawLine(frontTopRight, backTopRight, colour);
        DrawLine(frontBottomRight, backBottomRight, colour);
        DrawLine(frontBottomLeft, backBottomLeft, colour);
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawBox(Vector3 center, Vector3 dimensions, Quaternion rotation)
    {
        DrawBox(center, dimensions, rotation, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawBox(Box3 box)
    {
        DrawBox(box, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRectangle(Vector3 center, Vector2 dimensions, Quaternion rotation, Color colour)
    {
#if UNITY_EDITOR
        Vector3 extents = dimensions * 0.5f;

        Vector3 frontTopLeft = rotation * new Vector3(-extents.x, extents.y, 0) + center;
        Vector3 frontTopRight = rotation * new Vector3(extents.x, extents.y, 0) + center;
        Vector3 frontBottomLeft = rotation * new Vector3(-extents.x, -extents.y, 0) + center;
        Vector3 frontBottomRight = rotation * new Vector3(extents.x, -extents.y, 0) + center;

        DrawLine(frontTopLeft, frontTopRight, colour);
        DrawLine(frontTopRight, frontBottomRight, colour);
        DrawLine(frontBottomRight, frontBottomLeft, colour);
        DrawLine(frontBottomLeft, frontTopLeft, colour);
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRectangle(Vector3 center, Vector2 dimensions, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            if (UnityEditor.SceneView.lastActiveSceneView.in2DMode)
            {
                DrawRectangle(center, dimensions, Quaternion.LookRotation(Vector3.back), colour);
            }
            else
            {
                DrawRectangle(center, dimensions, Quaternion.LookRotation(UnityEditor.SceneView.lastActiveSceneView.camera.transform.position - center), colour);
            }
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRectangle(Vector3 center, Vector2 dimensions)
    {
        DrawRectangle(center, dimensions, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRectangle(Vector3 center, Vector2 dimensions, Quaternion rotation)
    {
        DrawRectangle(center, dimensions, rotation, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRectangle(Rect rect, Color colour)
    {
        DrawRectangle(rect.center, rect.size, colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawRectangle(Rect rect)
    {
        DrawRectangle(rect.center, rect.size, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSquare(Vector3 center, float width, Quaternion rotation, Color colour)
    {
        DrawRectangle(center, new Vector2(width, width), rotation, colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSquare(Vector3 center, float width, Quaternion rotation)
    {
        DrawRectangle(center, new Vector2(width, width), rotation, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSquare(Vector3 center, float width, Color colour)
    {
        DrawRectangle(center, new Vector2(width, width), colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawSquare(Vector3 center, float width)
    {
        DrawRectangle(center, new Vector2(width, width), Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, Vector3 normal, float capLength, Color colour)
    {
#if UNITY_EDITOR
        Quaternion rotation = Quaternion.LookRotation(normal);
        Vector3 capOffset = capLength * new Vector3(0.57735026919f, 0, 0); // Tan(30)
        Vector3 capStart = end - ((end - start).normalized * capLength);

        DrawLine(start, end, colour);
        DrawLine(capStart + rotation * capOffset, end, colour);
        DrawLine(capStart + rotation * -capOffset, end, colour);
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrowRay(Vector3 start, Vector3 direction, float capLength, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            Vector3 end = start + direction;
            Quaternion rotation = Quaternion.LookRotation((end - start).normalized, UnityEditor.SceneView.lastActiveSceneView.rotation.GetForwardVector());
            Vector3 capOffset = capLength * new Vector3(0.57735026919f, 0, 0); // Tan(30)
            Vector3 capStart = end - ((end - start).normalized * capLength);

            DrawLine(start, end, colour);
            DrawLine(capStart + rotation * capOffset, end, colour);
            DrawLine(capStart + rotation * -capOffset, end, colour);
        }
#endif

    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, float capLength, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            Quaternion rotation = Quaternion.LookRotation((end - start).normalized, UnityEditor.SceneView.lastActiveSceneView.rotation.GetForwardVector());
            Vector3 capOffset = capLength * new Vector3(0.57735026919f, 0, 0); // Tan(30)
            Vector3 capStart = end - ((end - start).normalized * capLength);

            DrawLine(start, end, colour);
            DrawLine(capStart + rotation * capOffset, end, colour);
            DrawLine(capStart + rotation * -capOffset, end, colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, Vector3 normal, Color colour)
    {
        DrawArrow(start, end, normal, 0.1f, colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, Color colour)
    {
        DrawArrow(start, end, 0.1f, colour);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, float capLength)
    {
        DrawArrow(start, end, capLength, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, float capLength, Vector3 normal)
    {
        DrawArrow(start, end, normal, capLength, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end)
    {
        DrawArrow(start, end, 0.1f, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawArrow(Vector3 start, Vector3 end, Vector3 normal)
    {
        DrawArrow(start, end, normal, 0.1f, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircle(Vector3 center, float radius, Vector3 normal, Color colour)
    {
#if UNITY_EDITOR
        int numSegments = Mathf.FloorToInt(Mathf.Clamp(Mathf.Sqrt(radius) * 36f, 16f, 360f));
        float interval = 360f / numSegments;

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, normal);
        Vector3 forward = Vector3.forward * radius;
        Vector3 lastVertex = center + rotation * forward;

        for (int i = 0; i <= numSegments; i++)
        {
            Vector3 vertex = center + rotation * Quaternion.Euler(0, interval * i, 0) * forward;
            DrawLine(lastVertex, vertex, colour);
            lastVertex = vertex;
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircle(Vector3 center, float radius, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            if (UnityEditor.SceneView.lastActiveSceneView.in2DMode)
            {
                DrawCircle(center, radius, Vector3.back, colour);
            }
            else
            {
                DrawCircle(center, radius, UnityEditor.SceneView.lastActiveSceneView.camera.transform.position - center, colour);
            }
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircle(Vector3 center, float radius, Vector3 normal)
    {
        DrawCircle(center, radius, normal, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircle(Vector3 center, float radius)
    {
        DrawCircle(center, radius, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularArc(Vector3 origin, Vector3 direction, float radius, float angle, Vector3 normal, Color colour)
    {
#if UNITY_EDITOR
        if (angle >= 360f)
        {
            DrawCircle(origin, radius, normal, colour);
        }
        else if (angle > 0)
        {
            int numSegments = Mathf.FloorToInt(Mathf.Clamp(radius * (angle / 10f), 8f, 360f));
            float interval = angle / numSegments;

            Quaternion rotation = Quaternion.LookRotation(direction.normalized, normal) * Quaternion.Euler(0, -angle * 0.5f, 0);
            Vector3 forward = Vector3.forward * radius;
            Vector3 lastVertex = origin + rotation * forward;

            for (int i = 0; i <= numSegments; i++)
            {
                Vector3 vertex = origin + rotation * Quaternion.Euler(0, interval * i, 0) * forward;
                DrawLine(lastVertex, vertex, colour);
                lastVertex = vertex;
            }
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularArc(Vector3 origin, Vector3 direction, float radius, float angle, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            DrawCircularArc(origin, direction, radius, angle, UnityEditor.SceneView.lastActiveSceneView.rotation.GetForwardVector(), colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularArc(Vector3 origin, Vector3 direction, float radius, float angle, Vector3 normal)
    {
        DrawCircularArc(origin, direction, radius, angle, normal, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularArc(Vector3 origin, Vector3 direction, float radius, float angle)
    {
        DrawCircularArc(origin, direction, radius, angle, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularSector(Vector3 origin, Vector3 direction, float radius, float angle, Vector3 normal, Color colour)
    {
#if UNITY_EDITOR
        if (angle >= 360f)
        {
            DrawCircle(origin, radius, normal, colour);
        }
        else if (angle > 0)
        {
            DrawCircularArc(origin, direction, radius, angle, normal, colour);

            Quaternion rotation = Quaternion.LookRotation(direction.normalized, normal);
            DrawLine(origin, origin + rotation * Quaternion.Euler(0, -angle * 0.5f, 0) * Vector3.forward * radius, colour);
            DrawLine(origin, origin + rotation * Quaternion.Euler(0, angle * 0.5f, 0) * Vector3.forward * radius, colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularSector(Vector3 origin, Vector3 direction, float radius, float angle, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            DrawCircularSector(origin, direction, radius, angle, UnityEditor.SceneView.lastActiveSceneView.rotation.GetForwardVector(), colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularSector(Vector3 origin, Vector3 direction, float radius, float angle, Vector3 normal)
    {
        DrawCircularSector(origin, direction, radius, angle, normal, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawCircularSector(Vector3 origin, Vector3 direction, float radius, float angle)
    {
        DrawCircularSector(origin, direction, radius, angle, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawAnnulusSector(Vector3 origin, Vector3 direction, float innerRadius, float outerRadius, float angle, Vector3 normal, Color colour)
    {
#if UNITY_EDITOR
        if (angle >= 360f)
        {
            DrawCircle(origin, innerRadius, normal, colour);
            DrawCircle(origin, outerRadius, normal, colour);
        }
        else if (angle > 0)
        {
            DrawCircularArc(origin, direction, innerRadius, angle, normal, colour);
            DrawCircularArc(origin, direction, outerRadius, angle, normal, colour);

            Quaternion rotationA = Quaternion.LookRotation(direction.normalized, normal) * Quaternion.Euler(0, -angle * 0.5f, 0);
            Quaternion rotationB = Quaternion.LookRotation(direction.normalized, normal) * Quaternion.Euler(0, angle * 0.5f, 0);

            DrawLine(origin + rotationA * Vector3.forward * innerRadius, origin + rotationA * Vector3.forward * outerRadius, colour);
            DrawLine(origin + rotationB * Vector3.forward * innerRadius, origin + rotationB * Vector3.forward * outerRadius, colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawAnnulusSector(Vector3 origin, Vector3 direction, float innerRadius, float outerRadius, float angle, Color colour)
    {
#if UNITY_EDITOR
        if (UnityEditor.SceneView.lastActiveSceneView != null)
        {
            DrawAnnulusSector(origin, direction, innerRadius, outerRadius, angle, UnityEditor.SceneView.lastActiveSceneView.rotation.GetForwardVector(), colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawAnnulusSector(Vector3 origin, Vector3 direction, float innerRadius, float outerRadius, float angle, Vector3 normal)
    {
        DrawAnnulusSector(origin, direction, innerRadius, outerRadius, angle, normal, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawAnnulusSector(Vector3 origin, Vector3 direction, float innerRadius, float outerRadius, float angle)
    {
        DrawAnnulusSector(origin, direction, innerRadius, outerRadius, angle, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawDottedLine(Vector3 start, Vector3 end, float dashLength, float gapLength, Color colour)
    {
#if UNITY_EDITOR
        if (dashLength <= 0 || gapLength <= 0) throw new ArgumentException();

        float length = (end - start).magnitude;
        Vector3 increment = (end - start) / length;

        for (float t = 0; t < length; t += dashLength + gapLength)
        {
            DrawLine(start + increment * t, start + increment * Mathf.Min(t + dashLength, length), colour);
        }
#endif
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawDottedLine(Vector3 start, Vector3 end, float dashLength, float gapLength)
    {
        DrawDottedLine(start, end, dashLength, gapLength, Color.white);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawDottedLine(Vector3 start, Vector3 end, Color colour)
    {
        DrawDottedLine(start, end, 1f, 1f, colour);
    }




    static Vector3 AxisAligendMidPoint(Vector3 start, Vector3 end)
    {

        Vector3 mid = (start + end) / 2;

        Vector3 diff = end - start;

        Vector3 diffAbs = diff;
        diffAbs.x = Mathf.Abs(diff.x);
        diffAbs.z = Mathf.Abs(diff.z);

        {
            if (diffAbs.x < diffAbs.z)
            {


                //    if (diff.x >0)
                {
                    mid.x = end.x;
                    mid.z = start.z;

                }

            }
            else
            {
                // if (diff.x < 0)
                {

                    mid.x = start.x;
                    mid.z = end.z;

                }
            }
        }

        return mid;
    }

    public static void DrawAxisAlignedLine(Transform from, Transform to)
    {
        DrawAxisAlignedLine(from, to, Color.white);
    }
    public static void DrawAxisAlignedLine(Transform from, Transform to, Color color)
    {

        DrawAxisAlignedLine(new Box3(from), new Box3(to), color);
    }
    public static void DrawAxisAlignedLine(Box3 from, Box3 to)
    {
        DrawAxisAlignedLine(from, to, Color.white);
    }
    public static void DrawAxisAlignedLine(Box3 from, Box3 to, Color color)
    {
        Vector3 f = from.Center;
        f.y = to.Center.y;
        from.Center = f;

        from.Extents = from.Extents.WithY(1);
        to.Extents = to.Extents.WithY(1);

        Vector3 start = from.Center;
        Vector3 end = to.Center;



        start.y = end.y;

        Vector3 diff = end - start;

        Vector3 diffAbs = diff;
        diffAbs.x = Mathf.Abs(diff.x);
        diffAbs.z = Mathf.Abs(diff.z);


        float a = 2f;
        float angle = Mathf.Abs(Vector3.Angle(Vector3.forward, end - start));
        if ((angle > 45 - a && angle < 45 + a) || (angle > 135 - a && angle < 135 + a))
        {
            //   Utils.DrawDirectionLine(start, end, Color.green, reverseLineDirection);

            float dist = diff.magnitude;

            Vector3 dir = (end - start);
            dir.x = Mathf.Abs(dir.z) * Mathf.Sign(dir.x);

            end = start + dir.normalized * dist;

            Vector3 mid = (start + end) / 2f;

            start = from.GetClosestPositionXZ(mid);
            end = to.GetClosestPositionXZ(mid);

            //   DebugUtils.DrawSphere(mid, .3f, Color.red);

            DrawDirectionLine(start, end, Color.white);
        }
        else
        {
            //   Vector3 midOriginal = AxisAligendMidPoint(start, end);

            //   Utils.DrawDirectionLine(from.Center, to.Center, Color.blue, reverseLineDirection);
            //    DebugUtils.DrawLine(midOriginal, end, Color.green);


            Vector3 mid = AxisAligendMidPoint(start, end);


            //Utils.DrawDirectionLine(start, mid, Color.green.WithAlpha(.2f), reverseLineDirection);

            //DebugUtils.DrawLine(mid, end, Color.green.WithAlpha(.2f));

            from.Extents *= 1.01f;
            to.Extents *= 1.01f;

            // bounds.size *= 1.05f;
            //    DebugUtils.DrawSphere(mid, .3f, Color.white);
            if (from.ContainsPoint(mid))
            {
                Vector3 A = MathUtils.ClosestPointOnBox(from, end);
                Vector3 B = MathUtils.ClosestPointOnBox(to, A);
                //    DebugUtils.DrawLine(mid, end, Color.red);
                DrawDirectionLine(A, B, color);

                // DebugUtils.DrawSphere(mid, .3f, Color.green);
            }
            else if (to.ContainsPoint(mid))
            {
                //    DebugUtils.DrawLine(mid, end, Color.red);
                Vector3 A = MathUtils.ClosestPointOnBox(to, start);
                Vector3 B = MathUtils.ClosestPointOnBox(from, A);

                DrawDirectionLine(A, B, color);

                //  DebugUtils.DrawSphere(mid, .3f, Color.red);
            }
            else
            {

                start = from.GetClosestPositionXZ(mid);
                end = to.GetClosestPositionXZ(mid);


                DrawDirectionLine(start, mid, color);
                //   Utils.DrawDirectionLine(start, mid, Color.white, reverseLineDirection);
                DrawDirectionLine(mid, end, color);

            }
        }
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawDirectionLine(Vector3 start, Vector3 end, Color colour, bool reverse = false)
    {
#if UNITY_EDITOR


        if (reverse)
        {
            Vector3 temp = start;
            start = end;
            end = temp;


        }


        DebugUtils.DrawLine(start, end, colour);


        float capLength = 0.5f;
        end = (start + end) / 2 + (end - start).normalized * capLength;

        if (end != start)
        {
            Quaternion rotation = Quaternion.LookRotation((end - start).normalized, UnityEditor.SceneView.lastActiveSceneView.rotation.GetForwardVector());
            Vector3 capOffset = capLength * new Vector3(0.57735026919f, 0, 0); // Tan(30)
            Vector3 capStart = end - ((end - start).normalized * capLength);
            DebugUtils.DrawLine(capStart + rotation * capOffset, end, colour);
            DebugUtils.DrawLine(capStart + rotation * -capOffset, end, colour);
        }
#endif
    }


    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void DrawDottedLine(Vector3 start, Vector3 end)
    {
        DrawDottedLine(start, end, 1f, 1f, Color.white);
    }


    /// <summary>
    /// Throws an error if a type is not an Enum type.
    /// </summary>
    /// <param name="T">The type to check</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertIsEnumType<T>(string errorMessage = null)
    {
        if (!typeof(T).IsEnum) { throw new UnityException(errorMessage == null ? "Assert failed, Type is not an enum!" : "Assert failed: " + errorMessage); }
    }

    /// <summary>
    /// Throws an error if a type is not an Enum type.
    /// </summary>
    /// <param name="type">The type to check</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertIsEnumType(Type type, string errorMessage = null)
    {
        if (!type.IsEnum) { throw new UnityException(errorMessage == null ? "Assert failed, Type is not an enum!" : "Assert failed: " + errorMessage); }
    }

    /// <summary>
    /// Throws an error if a float value is not in the 0-1 range. Method (and calls) are only compiled in if this is an editor build.
    /// </summary>
    /// <param name="value">The float to check</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertNormalized(float value, string errorMessage = null)
    {
        if (!(value >= 0 && value <= 1)) { throw new UnityException(errorMessage == null ? "Assert failed, value not normalized: " + value : "Assert failed: " + errorMessage); }
    }

    /// <summary>S
    /// Throws an error if an index is out of the valid range for an IList list. Method (and calls) are only compiled in if this is an editor build.
    /// </summary>
    /// <param name="index">The index to check</param>
    /// <param name="list">The list to check</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertValidIndex(int index, IList list, string errorMessage = null)
    {
        if (!(index >= 0 && index < list.Count)) { throw new UnityException(errorMessage == null ? "Assert failed, index is invalid: " + index : "Assert failed: " + errorMessage); }
    }

    /// <summary>
    /// Throws an error if a list is empty. Method (and calls) are only compiled in if this is an editor build.
    /// </summary>
    /// <param name="collection">The list to check</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertNotEmpty(ICollection collection, string errorMessage = null)
    {
        if (collection.Count <= 0) { throw new UnityException(errorMessage == null ? "Assert failed, list is empty." : "Assert failed: " + errorMessage); }
    }

    /// <summary>
    /// Throws an error if a float value is not in a specific range. Method (and calls) are only compiled in if this is an editor build.
    /// </summary>
    /// <param name="value">The float to check</param>
    /// <param name="lowerBound">The lower bound</param>
    /// <param name="upperBound">The upper bound</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertInRange(float value, float lowerBound, float upperBound, string errorMessage = null)
    {
        if (!(value >= lowerBound && value <= upperBound)) { throw new UnityException(errorMessage == null ? "Assert failed, value not in range: " + value : "Assert failed: " + errorMessage); }
    }

    /// <summary>
    /// Throws an error if a int value is not in a specific range. Method (and calls) are only compiled in if this is an editor build.
    /// </summary>
    /// <param name="value">The int to check</param>
    /// <param name="lowerBound">The lower bound</param>
    /// <param name="upperBound">The upper bound</param>
    /// <param name="errorMessage">The optional error message to print to the Unity console if the assertion fails</param>
    [System.Diagnostics.Conditional("UNITY_ASSERTIONS")]
    public static void AssertInRange(int value, int lowerBound, int upperBound, string errorMessage = null)
    {
        if (!(value >= lowerBound && value <= upperBound)) { throw new UnityException(errorMessage == null ? "Assert failed, value not in range: " + value : "Assert failed: " + errorMessage); }
    }


    /// <summary>
    /// Logs all the elements of a list out to the Unity console.
    /// </summary>
    /// <param name="collection">The list to log</param>
    /// <param name="oneLine">Whether or not to log the entire colleciton on one line</param>
    public static void LogCollection(IEnumerable collection, bool oneLine = true)
    {
        bool isEmpty = true;

        if (oneLine)
        {
            StringBuilder builder = new StringBuilder();

            foreach (object obj in collection)
            {
                isEmpty = false;
                builder.Append(obj);
                builder.Append(", ");
            }

            if (!isEmpty)
            {
                builder.Remove(builder.Length - 2, 2);
                Debug.Log(builder);
            }
        }
        else
        {
            foreach (object obj in collection)
            {
                isEmpty = false;
                Debug.Log(obj);
            }
        }

        if (isEmpty)
        {
            Debug.Log("Empty collection.");
        }
    }
}
