using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class DogController : MonoBehaviour
{
    

    private PlayerInput _playerInput;
    [SerializeField] private DogConfiguration _configuration;
    [SerializeField] private GamepadInput _controls;
    [SerializeField] private Transform _pullAnchor;
    [SerializeField] private Transform _buttPullAnchor;
    [SerializeField] private Rigidbody _chestRigidbody;
    [SerializeField] private Rigidbody _buttRigidbody;
    [SerializeField] private JumpScript _jump;
      [SerializeField] private BiteScript _biteScript;
    [SerializeField] private Animator[] _legs;
    private Vector3 _input;


    private void Awake()
    {
      
    }

    public Vector3 dogPosition => _chestRigidbody.transform.position;
    public Material color { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // _controls = new GamepadInput();
        // _controls.Gameplay.Jump.performed += OnMove;
        // _controls.Enable();
    }

    public void OnJump(InputAction.CallbackContext obj)
    {
        _jump.DoJump();
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Scene_Menu");
    }

    
    public void OnBite(InputAction.CallbackContext obj)
    {
        
        // _biteScript.DoBite();
            
        if (obj.performed)
        {
            _biteScript.DoBite();

        }
        else  if (obj.canceled)
        {
            _biteScript.DoRelease();
        }
        // _jump.DoJump();
        // Debug.Log("Bark");
    }
    
    
  
    
    public void OnMove(InputAction.CallbackContext obj)
    {
        Vector2 readValue = obj.ReadValue<Vector2>();

        _input = new Vector3(readValue.x, 0, readValue.y);
    }



    private void Update()
    {
        // Vector2 readValue = _controls.Gameplay.Movement.ReadValue<Vector2>();


         // _input = new Vector3(readValue.x, 0, readValue.y);
    
         foreach (var leg in _legs)
         {
             leg.SetBool("Run", _input.magnitude > 0);
         }
         
         
            Debug.DrawRay(_pullAnchor.transform.position, _input, Color.red);
        // _chestRigidbody.AddForceAtPosition();
    }

    private void FixedUpdate()
    {
        float dot = Vector3.Dot(_chestRigidbody.transform.forward, _input);

        // float directionModifier = _configuration.multiplyInputByDot.Evaluate(dot);
        _chestRigidbody.AddForceAtPosition(_input*_configuration.pullForce + Vector3.up*_configuration.upPullForce*_input.magnitude, _pullAnchor.transform.position);
        _buttRigidbody.AddForceAtPosition(-_input*_configuration.buttPullForce, _buttPullAnchor.transform.position);
    }

}
