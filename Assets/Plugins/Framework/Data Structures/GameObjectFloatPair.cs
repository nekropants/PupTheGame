using UnityEngine;
using System;

[Serializable]
public struct GameObjectFloatPair
{
    public GameObject GameObject { get { return _gameObject; } set { _gameObject = value; } }
    public float Number { get { return _number; } set { _number = value; } }

    [SerializeField]
    private GameObject _gameObject;
    [SerializeField]
    private float _number;

    public GameObjectFloatPair(GameObject gameObject, float number)
    {
        _gameObject = gameObject;
        _number = number;
    }
}

