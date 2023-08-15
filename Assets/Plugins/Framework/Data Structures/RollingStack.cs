﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Assertions;

/// <summary>
/// A data structure that maintains a rolling stack of elements with a maximum capacity. When a new item is pushed, if the collection is full, the last one is pushed off the stack.
/// </summary>
public class RollingStack<T> : IEnumerable<T>
{
    /// <summary>
    /// The number of items currently on the stack.
    /// </summary>
    public int Count { get { return _count; } }
    /// <summary>
    /// The maximum number of items allowed on the stack.
    /// </summary>
    public int Capacity { get { return _elements.Length; } }

    /// <summary>
    /// Gets the element at the specified index. Items that were most recently pushed are returned first.
    /// </summary>
    /// <param name="index">The index to check</param>
    /// <returns>The item at the specififed index</returns>
    public T this[int index]
    {
        get
        {
            if (index >= _count || index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            return _elements[MathUtils.Wrap(_startIndex - index, 0, _count - 1)];
        }
    }

    private T[] _elements;
    private int _count;
    private int _startIndex;

    /// <summary>
    /// Creates a new RollingStack with the specified capacity.
    /// </summary>
    /// <param name="capacity">The maximum number of items that will remain on the stack</param>
    public RollingStack(int capacity)
    {
        _elements = new T[capacity];
    }

    /// <summary>
    /// Returns the most recently pushed item on the stack.
    /// </summary>
    /// <returns></returns>
    public T Peek()
    {
        return this[0];
    }

    /// <summary>
    /// Pushes a new item onto the stack, pushing the last one off if necessary.
    /// </summary>
    /// <param name="item">The new item to push</param>
    public void Push(T item)
    {
        if (_count < _elements.Length)
        {
            _elements[_count] = item;
            _startIndex = _count;
            _count++;
        }
        else
        {
            _startIndex = MathUtils.Wrap(_startIndex + 1, 0, _elements.Length - 1);
            _elements[_startIndex] = item;
        }
    }

    /// <summary>
    /// Removes all the items from the stack.
    /// </summary>
    public void Clear()
    {
        _elements = new T[_elements.Length];
        _count = 0;
        _startIndex = 0;
    }

    /// <summary>
    /// Checks whether the stack contains an item.
    /// </summary>
    /// <param name="item">The item to check</param>
    /// <returns>True if the item was found</returns>
    public bool Contains(T item)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_elements[i].Equals(item))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Copies the items on the stack to an array. Starting at the specified index of the target array. Note that this is not particularly efficient because items are not stored in the order that they were pushed.
    /// </summary>
    /// <param name="array">The target array</param>
    /// <param name="arrayIndex">The starting index</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        T[] temp = new T[_count];
        for (int i = 0; i < _count; i++)
        {
            temp[i] = this[i];
        }

        temp.CopyTo(array, arrayIndex);
    }

    /// <summary>
    /// Gets an enumerator that enumerates through the items currently on the stack. In order of most recently added to least recently added.
    /// </summary>
    /// <returns>The stack enumerator</returns>
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return this[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return this[i];
        }
    }

}
