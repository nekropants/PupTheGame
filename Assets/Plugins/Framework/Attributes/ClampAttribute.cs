using UnityEngine;
using System.Collections;

/// <summary>
/// Forces an int or a float field to be clamped between two values.
/// </summary>
public class ClampAttribute : PropertyAttribute
{

    public float min = 0;
    public float max = 1;

    /// <summary>
    /// Forces an int or a float field to be clamped between zero and some maximum. 
    /// </summary>
    /// <param name="max">The maximum value</param>
    public ClampAttribute(float max = 1)
    {
        this.max = max;
    }

    /// <summary>
    /// Forces an int or a float field to be clamped between two values.
    /// </summary>
    /// <param name="min">The minimum value</param>
    /// <param name="max">The maximum value</param>
    public ClampAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

}
