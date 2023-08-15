using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

/// <summary>
/// Simple class to manage a pool of GameObjects parented to the same transform.
/// </summary>
public class ChildPool<T> : IEnumerable<T> where T : Component
{
    public int ElementCount { get { return _visibleElementCount; } }
    public int PoolSize { get { return _elementPool.Count; } }
    public T this[int index] { get { return GetElement(index); } }

    private List<T> _elementPool = new List<T>();
    private int _visibleElementCount;
    private Transform _parentTransform;
    private GameObject _elementPrefab;

    /// <summary>
    /// Creates a new Child pool of the specified MonoBehaviour type.
    /// </summary>
    /// <param name="parentTransform">The transform that all the elements will be childed to</param>
    /// <param name="elementPrefab">The prefab to instantiate for each prefab</param>
    /// <param name="initialPoolSize">The initial number of elements to instantiate in the pool</param>
    public ChildPool(Transform parentTransform, GameObject elementPrefab, int initialPoolSize = 5)
    {
        Assert.IsNotNull(parentTransform);
        Assert.IsNotNull(elementPrefab);
        Assert.IsTrue(initialPoolSize >= 0);
        Assert.IsNotNull(elementPrefab.GetComponentInChildren<T>());

        _parentTransform = parentTransform;
        _elementPrefab = elementPrefab;

        SetElementCount(initialPoolSize);
        Clear();
    }

    /// <summary>
    /// Finds the
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetElement(int index)
    {
        Assert.IsTrue(index >= 0);
        Assert.IsTrue(index < _visibleElementCount);

        return _elementPool[index];
    }

    public void Clear()
    {
        SetElementCount(0);
    }

    public T Add()
    {
        _visibleElementCount++;
        if (_visibleElementCount - 1 == _elementPool.Count)
        {
            return GrowPool();
        }
        _elementPool[_visibleElementCount - 1].gameObject.SetActive(true);
        return _elementPool[_visibleElementCount - 1];
    }

    public T Peek()
    {
        if (_visibleElementCount > 0)
        {
            return _elementPool[_visibleElementCount - 1];
        }
        return null;
    }

    public bool Remove(T item)
    {
        Assert.IsNotNull(item);

        if (_elementPool.Remove(item))
        {
            _elementPool.Add(item);
            item.gameObject.SetActive(false);
            _visibleElementCount--;
            return true;
        }
        return false;
    }


    public void SetElementCount(int numElements)
    {
        Assert.IsTrue(numElements >= 0);

        _visibleElementCount = numElements;

        for (int i = _elementPool.Count; i < _visibleElementCount; i++)
        {
            GrowPool();
        }

        for (int i = 0; i < _elementPool.Count; i++)
        {
            if (i < _visibleElementCount)
            {
                _elementPool[i].gameObject.SetActive(true);
            }
            else
            {
                _elementPool[i].gameObject.SetActive(false);
            }
        }
    }

    private T GrowPool()
    {
        T newElement = Object.Instantiate(_elementPrefab).GetComponentInChildren<T>();
        newElement.transform.SetParent(_parentTransform, false);
        newElement.transform.localScale = Vector3.one;
        _elementPool.Add(newElement);
        return newElement;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _visibleElementCount; i++)
        {
            yield return _elementPool[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
