using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

/// <summary>
/// Helps serialize arrays or lists where an enum will be used as an index.
/// </summary>
public class EnumIndexedAttribute : PropertyAttribute
{
    public Type Type { get { return _type; } }
    private Type _type;

    public EnumIndexedAttribute(Type type)
    {
        Assert.IsTrue(type.IsEnum);
        _type = type;
    }

}
