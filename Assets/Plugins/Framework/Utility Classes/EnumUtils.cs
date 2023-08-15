using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

/// <summary>
/// Utility class for enum things.
/// </summary>
public static class EnumUtils
{

    /// <summary>
    /// Checks the total number of items for an enum type.
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <returns>The number of enum values</returns>
    public static int GetCount<T>() where T : struct, IComparable, IConvertible, IFormattable
    {
        DebugUtils.AssertIsEnumType<T>();
        return Enum.GetValues(typeof(T)).Length;
    }

    /// <summary>
    /// Checks the total number of items for an enum type.
    /// </summary>
    /// <typeparam name="enumType">The enum type</typeparam>
    /// <returns>The number of enum values</returns>
    public static int GetCount(Type enumType)
    {
        DebugUtils.AssertIsEnumType(enumType);
        return Enum.GetValues(enumType).Length;
    }

    /// <summary>
    /// Returns the next value in an enum after a specific value. Or the first value if the current value is the last in the enum. ASSUMES THE ENUM VALUES START AT 0 AND ARE ALL ONE INTEGER APART.
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <param name="currentValue">The current enum value</param>
    /// <returns>The next (wrapping) value in the enum</returns>
    public static T GetNextValueWrapped<T>(T currentValue) where T : struct, IComparable, IConvertible, IFormattable
    {
        DebugUtils.AssertIsEnumType<T>();
        return (T)(object)MathUtils.Wrap((int)(object)currentValue + 1, 0, Enum.GetValues(typeof(T)).Length - 1);
    }

    /// <summary>
    /// Returns previous next value in an enum after a specific value. Or the last value if the current value is the first in the enum. ASSUMES THE ENUM VALUES START AT 0 AND ARE ALL ONE INTEGER APART.
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <param name="currentValue">The current enum value</param>
    /// <returns>The previous (wrapping) value in the enum</returns>
    public static T GetPreviousValueWrapped<T>(T currentValue) where T : struct, IComparable, IConvertible, IFormattable
    {
        DebugUtils.AssertIsEnumType<T>();
        return (T)(object)MathUtils.Wrap((int)(object)currentValue - 1, 0, Enum.GetValues(typeof(T)).Length - 1);
    }

    /// <summary>
    /// Checks the number items with unqiue underlying values for an enum type.
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <returns>The number of unique values</returns>
    public static int CountUnique<T>() where T : struct, IComparable, IConvertible, IFormattable
    {

        DebugUtils.AssertIsEnumType<T>();
        Array values = Enum.GetValues(typeof(T));
        ArrayList set = new ArrayList(values.Length);


        for (int i = 0; i < values.Length; i++)
        {
            if (!set.Contains(values.GetValue(i)))
            {
                set.Add(values.GetValue(i));
            }
        }

        return set.Count;
    }

    /// <summary>
    /// Checks the number items with positive integers as underlying values for an enum type. No.te that 0 is also treated as positive
    /// </summary>
    /// <typeparam name="T">The enum type, must have be an underlying int type</typeparam>
    /// <returns>The number of positive values</returns>
    public static int CountPositive<T>() where T : struct, IComparable, IConvertible, IFormattable
    {

        DebugUtils.AssertIsEnumType<T>();
        Assert.IsTrue(Enum.GetUnderlyingType(typeof(T)) == typeof(int));
        Array values = Enum.GetValues(typeof(T));
        int count = 0;

        for (int i = 0; i < values.Length; i++)
        {
            if ((int)values.GetValue(i) >= 0)
            {
                count++;
            }
        }

        return count;
    }

}
