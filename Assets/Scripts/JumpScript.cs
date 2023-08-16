using System.Collections;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private Rigidbody _front;
    [SerializeField] private Rigidbody _back;

    public void DoJump()
    {
        StartCoroutine(IEDoJump());
    }
    
    private IEnumerator IEDoJump()
    {
        _front.AddForce(Vector3.up*_configuration.frontJump, ForceMode.Impulse);
        yield return new WaitForSeconds(_configuration.jumpDelay);
        _back.AddForce(Vector3.up*_configuration.backJump, ForceMode.Impulse);
    }
}