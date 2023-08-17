using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpringConfig : MonoBehaviour
{
    [SerializeField] private bool _triggerOnEnable;
    [SerializeField] private SpringConfiguration _springConfig;
    [SerializeField]  private SpringController _springController;

    public SpringConfiguration springConfig => _springConfig;

    private void OnEnable()
    {
        if (_triggerOnEnable)
        {
            ApplyConfig();
        }
    }

    public void ApplyConfig()
    {
        if(_springController== null)
            _springController = GetComponent<SpringController>();
        
        _springController.SetConfig(springConfig);
    }
}
