using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;


    public void Trigger()
    {
        _animator.SetTrigger(_triggerName);
    }
}
