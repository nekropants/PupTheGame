using UnityEngine;
using System;

[Serializable]
public struct GameObjectIntPair
{
    public GameObject GameObject { get { return _gameObject; } set { _gameObject = value; } }
    public int Number { get { return _number; } set { _number = value; } }

    [SerializeField]
    private GameObject _gameObject;
    [SerializeField]
    private int _number;

    public GameObjectIntPair(GameObject gameObject, int number)
    {
        _gameObject = gameObject;
        _number = number;
    }
}

