using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Contains extension methods for collections.
/// </summary>
public static class CollectionExtensions
{


    public static object[] ToArray(this ICollection collection)
    {
        object[] array = new object[collection.Count];
        int index = 0;

        foreach (object element in collection)
        {
            array[index++] = element;
        }

        return array;
    }

    /// <summary>
    /// Shorthand method for Array.AsReadOnly(array)
    /// </summary>
    /// <returns> Returns a read only collection of the array elements</returns>
    public static ReadOnlyCollection<T> AsReadOnly<T>(this T[] array)
    {
        // HACK: if array is null this causes an error, should probably check for this earlier.
        if (array == null)
        {
            Debug.LogWarning("Array is null");
            return new ReadOnlyCollection<T>(new List<T>());
        }

        return Array.AsReadOnly(array);
    }

    /// <summary>
    /// Randomizes the order of the elements in the collection. Uses Fisher-Yates algorithm.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static string GetElementsAsString(this IEnumerable collection)
    {
        StringBuilder builder = new StringBuilder();

        foreach (object obj in collection)
        {
            builder.Append(obj);
            builder.Append(", ");
        }

        if (builder.Length > 0)
        {
            return builder.ToString().Substring(0, builder.Length - 2);
        }

        return "NO ELEMENTS";
    }

    public static T[] SubArray<T>(this T[] array, int startIndex, int length)
    {
        T[] subset = new T[length];
        Array.Copy(array, startIndex, subset, 0, length);
        return subset;
    }

    /// <summary>
    /// Returns an array containing the elements in the collection, but in a random order. Uses Fisher-Yates algorithm.
    /// </summary>
    /// <returns>The new shuffled array</returns>
    public static T[] Shuffled<T>(this IList<T> list)
    {
        T[] array = new T[list.Count];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = list[i];
        }

        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }

        return array;
    }

    /// <summary>
    /// Removes the first occurence of an element from an array. Does so by performing at least one (usually two) array copies.
    /// </summary>
    /// <param name="element">The element to remove</param>
    /// <param name="result">The output array with the element removed</param>
    /// <returns>Whether or not the element was found and removed</returns>
    public static bool Remove<T>(this T[] array, T element, out T[] result)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Equals(element))
            {
                result = new T[array.Length - 1];

                if (i > 0)
                {
                    Array.Copy(array, 0, result, 0, i);
                }

                if (i < array.Length - 1)
                {
                    Array.Copy(array, i + 1, result, i, array.Length - i - 1);
                }

                return true;
            }
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Reverses the order of the elements in the collection.
    /// </summary>
    public static void Reverse<T>(this IList<T> list)
    {

        for (int i = 0; i < list.Count * 0.5f; i++)
        {
            T temp = list[list.Count - 1 - i];
            list[list.Count - 1 - i] = list[i];
            list[i] = temp;
        }

    }

    /// <summary>
    /// Gets the element at an index that will be wrapped if it is out of range.
    /// </summary>
    /// <param name="index"> The index to get</param>
    /// <returns> The value at the wrapped index</returns>
    public static T GetWrapped<T>(this IList<T> list, int index)
    {
        Assert.IsTrue(list.Count > 0);

        return list[MathUtils.Wrap(index, 0, list.Count - 1)];
    }

    /// <summary>
    /// Removes and returns the item at the end of the list.
    /// </summary>
    public static T Pop<T>(this IList<T> list)
    {
        Assert.IsTrue(list.Count > 0);

        T result = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return result;
    }

    /// <summary>
    /// Fill the list with a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="value">Value to fill the list with</param>
    /// <param name="count">Number of values to fill</param>
    /// <param name="extend">When false the list will be cleared, when true the list will be filled until the list length reaches count</param>
    public static void Fill<T>(this IList<T> list, T value, int count, bool extend)
    {
        if (!extend)
            list.Clear();

        for (int i = 0; i < count; ++i)
        {
            list.Add(value);
        }
    }

    /// <summary>
    /// Gets the value of the entry at specific index position in the dictionary, can be a useful shorthand for when you are iterating through all the entires in a dictionary.
    /// </summary>
    /// <typeparam name="K">Key type</typeparam>
    /// <typeparam name="V">Value type</typeparam>
    /// <param name="index">The index to lookup</param>
    /// <returns>The value retrieved</returns>
    public static V GetValueAt<K, V>(this Dictionary<K, V> dict, int index)
    {

        int count = 0;
        foreach (KeyValuePair<K, V> kvp in dict)
        {
            if (count == index)
            {
                return kvp.Value;
            }
            count++;
        }
        throw new Exception("Dictionary does not contain a value at index " + index);
    }

    /// <summary>
    /// Gets the key of the entry at specific index position in the dictionary, can be a useful shorthand for when you are iterating through all the entires in a dictionary.
    /// </summary>
    /// <typeparam name="K">Key type</typeparam>
    /// <typeparam name="V">Value type</typeparam>
    /// <param name="index">The index to lookup</param>
    /// <returns>The key retrieved</returns>
    public static K GetKeyAt<K, V>(this Dictionary<K, V> dict, int index)
    {
        int count = 0;
        foreach (KeyValuePair<K, V> kvp in dict)
        {
            if (count == index)
            {
                return kvp.Key;
            }
            count++;
        }
        throw new Exception("Dictionary does not contain a key at index " + index);
    }

    /// <summary>
    /// Returns whether or not the collection has no elements.
    /// </summary>
    /// <returns>Whether or not the collection has no elements</returns>
    public static bool IsEmpty<T>(this ICollection<T> collection)
    {
        return collection.Count == 0;
    }

    /// <summary>
    /// Clones the collection.
    /// </summary>
    /// <returns>The cloned collection</returns>
    public static IEnumerable<T> Clone<T>(this IEnumerable<T> collection) where T : ICloneable
    {
        return collection.Select(item => (T)item.Clone());
    }
}
