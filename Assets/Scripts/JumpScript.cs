using System.Collections;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private Rigidbody _front;
    [SerializeField] private Rigidbody _mid;
    [SerializeField] private Rigidbody _back;
    [SerializeField] private ColliderDetector _groundDetector;
    [SerializeField] private ParticleSystem _jumpParticles;


    public void DoJump()
    {
        if (_groundDetector.hasColliders)
        {
            StartCoroutine(IEDoJump());
        }
    }
    
    private IEnumerator IEDoJump()
    {

        // Physics.Raycast( _jumpParticles.transform.position + Vector3.up * 0.1f, Vector3.down,  out RaycastHit hit, 0.4f,1<< LayerMask.NameToLayer("Default"));

        // if (hit.collider)
        {
            _jumpParticles.transform.forward = Vector3.up;
        }
        _jumpParticles.Play();
        _front.AddForce(Vector3.up*_configuration.frontJumpUp, ForceMode.Impulse);
        _front.AddForce(_front.transform.forward*_configuration.frontJumpForward, ForceMode.Impulse);
        
        yield return new WaitForSeconds(_configuration.jumpDelay/2f);
        
        _front.AddForce(Vector3.up*_configuration.midJumpUp, ForceMode.Impulse);
        _front.AddForce(_front.transform.forward*_configuration.midJumpForward, ForceMode.Impulse);
        
        yield return new WaitForSeconds(_configuration.jumpDelay/2f);
        
        _back.AddForce(Vector3.up*_configuration.backJumpUp, ForceMode.Impulse);
        _back.AddForce(_front.transform.forward*_configuration.backJumpForward, ForceMode.Impulse);
    }
}