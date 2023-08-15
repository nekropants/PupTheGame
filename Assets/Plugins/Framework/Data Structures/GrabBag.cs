﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// A data structure for random sampling from a set of N choices with a guarantee the every element will be slected at least once every N samples. Also called a shuffle bag.
/// </summary>
/// <typeparam name="T">The type of the choices</typeparam>
public class GrabBag<T>
{
    /// <summary>
    /// The number of total choices in the GrabBag.
    /// </summary>
    public int NumChoices { get { return _elements.Count; } }

    protected List<T> _elements;
    protected int _cursor;

    /// <summary>
    /// Creates a new GrabBag from a params array of choices.
    /// </summary>
    /// <param name="choices">The choices in the bag</param>
    public GrabBag(params T[] choices)
    {
        _elements = new List<T>(choices);
        _cursor = _elements.Count - 1;
    }

    public GrabBag()
    {
        _elements = new List<T>();
    }

    /// <summary>
    /// Creates a new GrabBag from a IList of choices.
    /// </summary>
    /// <param name="choices">The choices in the bag</param>
    public GrabBag(IList<T> choices)
    {
        _elements = new List<T>(choices);
    }

    /// <summary>
    /// Gets the next random sample from the GrabBag.
    /// </summary>
    /// <returns>A random element from the bag</returns>
    public T GetNext()
    {
        if (_elements.Count == 0)
        {
            throw new UnityException("GrabBag has no choices!");
        }

        if (_cursor < 1)
        {
            _cursor = _elements.Count - 1;
            return _elements[0];
        }

        int grabIndex = Mathf.FloorToInt(Random.value * (_cursor + 1));

        T temp = _elements[grabIndex];
        _elements[grabIndex] = _elements[_cursor];
        _elements[_cursor] = temp;

        _cursor--;

        return temp;
    }

    /// <summary>
    /// Adds a new item to the possible pool in a random position.
    /// </summary>
    /// <param name="item">The new element to add</param>
    public void Add(T item)
    {
        _elements.Insert(Random.Range(0, _elements.Count), item);
    }


}

/// <summary>
/// A GrabBag with a range of integers as possible choices.
/// </summary>
public class GrabBag : GrabBag<int>
{
    /// <summary>
    /// Creates a new int range grab bag.
    /// </summary>
    /// <param name="min">The minimum possible value that can be selected (inclusive)</param>
    /// <param name="max">The maximum possible value that can be selected (inclusive)</param>
    public GrabBag(int min, int max)
    {
        _elements = new List<int>();
        for (int i = 0; i < max - min + 1; i++)
        {
            _elements.Add(min + i);
        }

        _cursor = _elements.Count - 1;
    }
}

/// <summary>
/// A GrabBag with enum values as possible choices.
/// </summary>
/// <typeparam name="T">The enum type</typeparam>
public class EnumGrabBag<T> : GrabBag<T> where T : struct, IComparable, IConvertible, IFormattable
{

    /// <summary>
    /// Creates a new EnumGrabBag with all possible enum values.
    /// </summary>
    public EnumGrabBag()
    {
        DebugUtils.AssertIsEnumType<T>();

        _elements = new List<T>((T[])Enum.GetValues(typeof(T)));
    }

    /// <summary>
    /// Creates a new EnumGrabBag with some enum values excluded.
    /// </summary>
    /// <param name="excludedValues">The excluded values, these are not put into the bag</param>
    public EnumGrabBag(params T[] excludedValues)
    {
        DebugUtils.AssertIsEnumType<T>();

        _elements = new List<T>();

        T[] values = (T[])Enum.GetValues(typeof(T));
        for (int i = 0; i < values.Length; i++)
        {
            bool isExcluded = false;
            for (int j = 0; j < excludedValues.Length; j++)
            {
                if (excludedValues[j].Equals(values[i]))
                {
                    isExcluded = true;
                    break;
                }
            }

            if (!isExcluded)
            {
                _elements.Add(values[i]);
            }
        }

        _cursor = _elements.Count - 1;
    }
}

