using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _sprintMultiplier = 1.5f;
    [SerializeField] private float _mouseSensitivity = 3.0f;
    [SerializeField] private bool _invertY = false;
    [SerializeField] private bool _lockMouseOnAwake = true;
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _gravityForce = 3.0f;
    [SerializeField] private float _startingYRotation = 0.0f;
    [SerializeField] private bool _canJump = true;
	
    [Header(("Bobbing")),SerializeField]
    private BobbingSettings _headBobSettings;
    [SerializeField] private float _cameraDefaultYPos = 1.5f;
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private BobbingSettings _weaponBobSettings;
    
    public float movementMagnitude { get; private set; }
    public bool isSprinting { get; private set; }

    private bool _isActive = true;
    private CharacterController _controller;
    private Vector3 _movementVelocity;
    private Vector3 _additiveVelocity;
    private Vector2 _directionInput;
    private Vector2 _mouseRotationInput;
    private bool _isMoving;
    private float _verticalVelocity;
    private Vector3 _headbobDestination;
    private Vector3 _weaponbobDestination;
    private Vector3 _weaponHolderDefaultPos;
    #endregion
    
    private void Awake()
    {
        GameManager.OnGamePaused += OnGamePaused;
    
        _mouseRotationInput = new Vector2(_startingYRotation, 0.0f);
        _controller = GetComponent<CharacterController>();
		
		if(_weaponHolder != null)
			_weaponHolderDefaultPos = _weaponHolder.transform.localPosition;
    }

    private void OnGamePaused(bool state)
    {
        _isActive = !state;
        ToggleMouseLock(!state);
    }

    private void Start()
    {
        ToggleMouseLock(_lockMouseOnAwake);
    }

    private void Update()
    {
        if (!_isActive) return;
        
        _additiveVelocity = Vector3.zero;

        isSprinting = Input.GetButton("Sprint");

        if (!_controller.isGrounded || !_isMoving)
        {
            _headbobDestination = new Vector3(0.0f, _cameraDefaultYPos, 0.0f);
            _weaponbobDestination = _weaponHolderDefaultPos;
        }
        
        _playerCamera.localPosition = Vector3.Lerp(_playerCamera.localPosition, _headbobDestination, _headBobSettings._bobLerp);
		
		if(_weaponHolder != null)
		{
			_weaponHolder.localPosition = Vector3.Lerp(_weaponHolder.localPosition, _weaponbobDestination, _weaponBobSettings._bobLerp);
		}
        
        if (Input.GetButton("Jump") && _canJump)
        {
            if (_controller.isGrounded)
            {
                _verticalVelocity = _jumpForce;
            }
        }
        
        //Cursor control
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            ToggleMouseLock(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMouseLock(false);
        }
        
        FinalizeMovement();
    }

    private void CalculateMovement()
    {
        _directionInput = new Vector2(Input.GetAxis("MovementX"), Input.GetAxis("MovementY"));
    }

    private void CalculateRotation()
    {
        Vector2 input = new Vector2(Input.GetAxis("LookX"), Input.GetAxis("LookY"));

        _mouseRotationInput += input * (_mouseSensitivity * Time.deltaTime);
        _mouseRotationInput.y = Mathf.Clamp(_mouseRotationInput.y, -90, 90);
        
        transform.rotation = Quaternion.AngleAxis(_mouseRotationInput.x, Vector3.up);
        _playerCamera.localRotation = Quaternion.AngleAxis(_mouseRotationInput.y * (_invertY ? 1 : -1), Vector3.right);
    }

    private void FinalizeMovement()
    {
        CalculateMovement();
        CalculateRotation();
        
        //Directional movement
        _movementVelocity = (transform.forward * _directionInput.y) + (transform.right * _directionInput.x);
        _movementVelocity = Vector3.ClampMagnitude(_movementVelocity, 1.0f);
        ApplyHeadBob(_movementVelocity.magnitude);
        ApplyWeaponBob(_movementVelocity.magnitude);
        movementMagnitude = _movementVelocity.magnitude;
        
        _movementVelocity += _additiveVelocity;
        _movementVelocity *= _movementSpeed * (isSprinting ? _sprintMultiplier : 1.0f);
        
        _isMoving = _movementVelocity.magnitude != 0 ? true : false;
        
        //Vertical movement
        _movementVelocity.y = _verticalVelocity;

        //External forces
        CalculateExternalForces();

        //Applying final movement
        _controller.Move(_movementVelocity + _additiveVelocity);
    }

    private void CalculateExternalForces()
    {
        if (!_controller.isGrounded)
        {
            _verticalVelocity -= _gravityForce * Time.fixedDeltaTime;
        }
    }

    private void ApplyHeadBob(float magnitude)
    {
        if (!_controller.isGrounded)
        {
            return;
        }

        float sinX = Mathf.Sin(Time.time * _headBobSettings._bobFrequencyX * (isSprinting ? _headBobSettings._sprintBobMultiplier : 1.0f)) * (magnitude * _headBobSettings._bobMagnitudeX);
        float sinY = Mathf.Sin(Time.time * _headBobSettings._bobFrequencyY * (isSprinting ? _headBobSettings._sprintBobMultiplier : 1.0f)) * (magnitude * _headBobSettings._bobMagnitudeY);
        float sinZ = Mathf.Sin(Time.time * _headBobSettings._bobFrequencyZ * (isSprinting ? _headBobSettings._sprintBobMultiplier : 1.0f)) * (magnitude * _headBobSettings._bobMagnitudeZ);
        
        _headbobDestination = new Vector3(sinX, _cameraDefaultYPos + sinY, sinZ);
    }

    private void ApplyWeaponBob(float magnitude)
    {
        if (!_controller.isGrounded)
        {
            return;
        }

        float sinX = Mathf.Sin(Time.time * _weaponBobSettings._bobFrequencyX * (isSprinting ? _weaponBobSettings._sprintBobMultiplier : 1.0f)) * (magnitude * _weaponBobSettings._bobMagnitudeX);
        float sinY = Mathf.Sin(Time.time * _weaponBobSettings._bobFrequencyY * (isSprinting ? _weaponBobSettings._sprintBobMultiplier : 1.0f)) * (magnitude * _weaponBobSettings._bobMagnitudeY);
        float sinZ = Mathf.Sin(Time.time * _weaponBobSettings._bobFrequencyZ * (isSprinting ? _weaponBobSettings._sprintBobMultiplier : 1.0f)) * (magnitude * _weaponBobSettings._bobMagnitudeZ);
        
        _weaponbobDestination = new Vector3(_weaponHolderDefaultPos.x + sinX, _weaponHolderDefaultPos.y + + sinY, _weaponHolderDefaultPos.z + sinZ);
    }
    
    private void ToggleMouseLock(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ForceKillPlayer()
    {
        Debug.Log("Death");
    }
}

[Serializable]
public class BobbingSettings
{
    public float _bobFrequencyX = 5.0f;
    public float _bobMagnitudeX = .4f;
    public float _bobFrequencyY = 10.0f;
    public float _bobMagnitudeY = .325f;
    public float _bobFrequencyZ = 0.0f;
    public float _bobMagnitudeZ = 0.0f;
    public float _sprintBobMultiplier = 1.75f;
    public float _bobLerp = .2f;
}