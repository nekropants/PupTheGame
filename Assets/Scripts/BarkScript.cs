using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkScript : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private Rigidbody _head;
    [SerializeField] private Transform _barkForcePoint;
    [SerializeField] private ParticleSystem _barkParticle;
    [SerializeField] private BiteScript _biteScript;
    [SerializeField] private AudioSource _barkAudioSource;
    [SerializeField] private AudioClip[] _barkClips;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IEBark());
    }

    private IEnumerator IEBark()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 12f));
            if (_biteScript.isBiting == false)
            {
                DoBark();            
            }
         
        }
    }

    // Update is called once per frame
    private void DoBark()
    {
        _head.AddForceAtPosition(_barkForcePoint.forward*_configuration.barkForce, _barkForcePoint.position, ForceMode.Impulse);
        _barkParticle.Play();
        var barkClip = _barkClips[Random.Range(0, _barkClips.Length)];
        _barkAudioSource.PlayOneShot(barkClip);
    }
}
