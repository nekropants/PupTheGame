using System.Collections;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private Rigidbody _front;
    [SerializeField] private Rigidbody _mid;
    [SerializeField] private Rigidbody _back;

    public void DoJump()
    {
        StartCoroutine(IEDoJump());
    }
    
    private IEnumerator IEDoJump()
    {
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