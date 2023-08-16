using UnityEngine;

[CreateAssetMenu(fileName = "Dog Configuration")]
public class DogConfiguration : ScriptableObject
{
    [SerializeField] private float _pullForce;

    [SerializeField] private float _buttPullForce;
    // [SerializeField] private SmartCurve _multiplyInputByDot;

    public float pullForce => _pullForce;

    public float buttPullForce
    {
        get => _buttPullForce;
        set => _buttPullForce = value;
    }

    // public SmartCurve multiplyInputByDot
    // {
    //     get => _multiplyInputByDot;
    // }
}