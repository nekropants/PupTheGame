
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TemporaryRandomSeed : IDisposable
{
    private Random.State _orignialState;

    public TemporaryRandomSeed(int seed)
    {
        _orignialState = Random.state;
        Random.InitState(seed);
    }

    public TemporaryRandomSeed() : this(new System.Random().Next())
    {

    }

    public void Dispose()
    {
        Random.state = _orignialState;
    }
}
