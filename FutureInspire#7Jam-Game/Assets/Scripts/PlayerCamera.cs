using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1f;
    [SerializeField] private float _maxAnglesDifference = 30f;
    [SerializeField] private Transform _player;
    [SerializeField] private InputActionReference _cameraInput;
    [SerializeField] private InputActionReference _freeCameraInput;
    private Vector3 _eulerAngles;
    private Vector3 _playerEulerAngles;
    private Vector3 _startFreeCameraEulerAngles;
    private bool _freeCamera = false;

    void Start()
    {
        _freeCameraInput.action.started += EnableFreeCamera;
        _freeCameraInput.action.canceled += DisableFreeCamera;
        Cursor.lockState = CursorLockMode.Locked;
        _playerEulerAngles = _player.eulerAngles;
    }

    void Update()
    {
        Vector2 mouseInput = _cameraInput.action.ReadValue<Vector2>();
        _eulerAngles += new Vector3(-mouseInput.y, mouseInput.x, 0) * _sensitivity * Time.deltaTime;
        _eulerAngles = new Vector3(Math.Clamp(_eulerAngles.x, -15, 30), _eulerAngles.y, _eulerAngles.z);
        transform.eulerAngles = _eulerAngles;

        if (!_freeCamera)
        {
            float anglesDifference = _eulerAngles.y - _playerEulerAngles.y;
            if (anglesDifference >= _maxAnglesDifference || anglesDifference <= -_maxAnglesDifference)
            {
                _playerEulerAngles.y += mouseInput.x * _sensitivity * Time.deltaTime;
                _player.eulerAngles = _playerEulerAngles;
            }
        }

        transform.position = _player.position;
    }

    void EnableFreeCamera(InputAction.CallbackContext context)
    {
        _freeCamera = true;
        _startFreeCameraEulerAngles = _eulerAngles;
    }

    void DisableFreeCamera(InputAction.CallbackContext context)
    {
        _freeCamera = false;
        _eulerAngles = _startFreeCameraEulerAngles;
    }

    void OnDestroy()
    {
        _freeCameraInput.action.started -= EnableFreeCamera;
        _freeCameraInput.action.canceled -= DisableFreeCamera;
    }
}
