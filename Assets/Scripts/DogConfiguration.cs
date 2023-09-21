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
     [SerializeField] private float _barkForce = 1;
     [SerializeField] private double _prizeGivingDuration = 30;


     [SerializeField] private SceneReference[] _levels;
     [SerializeField] private float _biteHeadKickBack;
     [SerializeField] private float _spitItOutForce;

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

    public float barkForce
    {
        get => _barkForce;
        set => _barkForce = value;
    }

    public double prizeGivingDuration
    {
        get => _prizeGivingDuration;
    }

    public SceneReference[] levels => _levels;
    public SceneReference activeLevel { get; set; }

    public float biteHeadKickBack
    {
        get => _biteHeadKickBack;
        set => _biteHeadKickBack = value;
    }

    public float spitItOutForce
    {
        get => _spitItOutForce;
        set => _spitItOutForce = value;
    }

    // public SmartCurve multiplyInputByDot
    // {
    //     get => _multiplyInputByDot;
    // }
}