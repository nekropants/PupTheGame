using UnityEngine;
using System.Collections;

/// <summary>
/// Forces an int or a float field to be higher than some minimum value. 
/// </summary>
public class MinValueAttribute : PropertyAttribute
{
    public float Value { get { return _value; } }

    private float _value = 0;

    /// <summary>
    /// Forces an int or a float field to be higher than some minimum value. 
    /// </summary>
    /// <param name="max">The maximum value</param>
    public MinValueAttribute(float value)
    {
        _value = value;
    }


}
