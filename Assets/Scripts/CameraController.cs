using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private Transform _target;
    private List<DogController> _dogs = new List<DogController>();
    // Start is called before the first frame update
    void Start()
    {
        _playerInputManager.onPlayerJoined += OnPlayerJoined;
        _playerInputManager.onPlayerLeft += OnPlayerLeft;
    }

    private void OnPlayerLeft(PlayerInput obj)
    {
        _dogs.Remove( obj.gameObject.GetComponent<DogController>());
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        _dogs.Add( obj.gameObject.GetComponent<DogController>());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 midPoint = default;
        int count = 0;
        foreach (DogController controller in _dogs)
        {
            if (controller)
            {
                count++;
                midPoint += controller.dogPosition;
            }
        }

        if (count > 0)
        {
            midPoint /= count;
            _target.transform.position = midPoint;
        }
    }
}
