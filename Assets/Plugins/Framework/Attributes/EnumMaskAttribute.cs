using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnumMaskAttribute : PropertyAttribute
{
    public Type Type { get { return _type; } }
    private Type _type;

    public EnumMaskAttribute(Type type)
    {
        Assert.IsTrue(type.IsEnum);
        _type = type;
    }
}
