using UnityEngine;

public class SetCenterOfMass : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _transform;

    private void FixedUpdate()
    {
        _rigidbody.centerOfMass = transform.InverseTransformPoint( _transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.01f);
    }
}