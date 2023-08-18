using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dog Configuration")]
public class DogConfiguration : ScriptableObject
{
    [SerializeField] private float _pullForce;
    [SerializeField] private float _upPullForce;

    [SerializeField] private float _buttPullForce;

    [SerializeField] private float _jumpDelay = 0.1f;
    [FormerlySerializedAs("_frontJump")] [SerializeField] private float _frontJumpUp = 1f;
    [SerializeField] private float _frontJumpForward = 1f;
    
    
      [SerializeField] private float _midJumpUp = 1f;
     [SerializeField] private float _midJumpForward = 1f;
     
     [FormerlySerializedAs("_backJump")] [SerializeField] private float _backJumpUp = 1f;
     [SerializeField] private float _backJumpForward = 1f;

    public float pullForce => _pullForce;
    public float upPullForce => _upPullForce;

    public float buttPullForce
    {
        get => _buttPullForce;
        set => _buttPullForce = value;
    }

    public float jumpDelay => _jumpDelay;

    public float frontJumpUp => _frontJumpUp;

    public float backJumpUp => _backJumpUp;

    public float frontJumpForward => _frontJumpForward;

    public float backJumpForward => _backJumpForward;

    public float midJumpForward => _midJumpForward;

    public float midJumpUp => _midJumpUp;

    // public SmartCurve multiplyInputByDot
    // {
    //     get => _multiplyInputByDot;
    // }
}