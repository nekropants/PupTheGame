using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DogController : MonoBehaviour
{
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private GamepadInput _controls;
    [SerializeField] private Transform _pullAnchor;
    [SerializeField] private Transform _buttPullAnchor;
    [SerializeField] private Rigidbody _chestRigidbody;
    [SerializeField] private Rigidbody _buttRigidbody;
    [SerializeField] private InputActionAsset _action;
    private Vector3 _input;

    // Start is called before the first frame update
    void Start()
    {
        _controls = new GamepadInput();
        _controls.Gameplay.Bark.performed += Bark;
        _controls.Gameplay.Movement.performed += Bark;
        _controls.Enable();
    }

    private void Bark(InputAction.CallbackContext obj)
    {
        
        // Debug.Log("Bark");
    }

    private void Update()
    {
        Vector2 readValue = _controls.Gameplay.Movement.ReadValue<Vector2>();
        Debug.Log(readValue);


         _input = new Vector3(readValue.x, 0, readValue.y);

         
         
            Debug.DrawRay(_pullAnchor.transform.position, _input, Color.red);
        // _chestRigidbody.AddForceAtPosition();
    }

    private void FixedUpdate()
    {
        float dot = Vector3.Dot(_chestRigidbody.transform.forward, _input);

        // float directionModifier = _configuration.multiplyInputByDot.Evaluate(dot);
        _chestRigidbody.AddForceAtPosition(_input*_configuration.pullForce, _pullAnchor.transform.position);
        _buttRigidbody.AddForceAtPosition(-_input*_configuration.buttPullForce, _buttPullAnchor.transform.position);
    }

}
