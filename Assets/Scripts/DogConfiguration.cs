using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dog Configuration")]
public class DogConfiguration : ScriptableObject
{
    [SerializeField] private float _pullForce;

    [SerializeField] private float _buttPullForce;

    [SerializeField] private float _jumpDelay = 0.1f;
    [SerializeField] private float _frontJump = 1f;
    [SerializeField] private float _backJump = 1f;

    public float pullForce => _pullForce;

    public float buttPullForce
    {
        get => _buttPullForce;
        set => _buttPullForce = value;
    }

    public float jumpDelay => _jumpDelay;

    public float frontJump => _frontJump;

    public float backJump => _backJump;

    // public SmartCurve multiplyInputByDot
    // {
    //     get => _multiplyInputByDot;
    // }
}