using UnityEngine;

public class SpringConfiguration : MonoBehaviour
{
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] ConfigurableJoint _spring;

    public Rigidbody rigidbody
    {
        get => _rigidbody;
    }
    public ConfigurableJoint spring
    {
        get => _spring;
    }
}