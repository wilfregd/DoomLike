using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponDrop : MonoBehaviour
{
    [SerializeField] private Transform _itemVisual;
    [SerializeField] private float _rotationSpeed = 2.0f;
    [SerializeField] private float _visualStartY = 1.25f;
    [SerializeField] private float _sineSpeed = 1.0f;
    [SerializeField] private float _sineMagnitude = 1.0f;
    [SerializeField] private AudioClip _pickupClip;
    [SerializeField] private GameObject _fpsWeaponPrefab;
    
    private AudioSource _source;
    private Vector3 _defaultVisualPos;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _defaultVisualPos = _itemVisual.localPosition;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

        float sine = Mathf.Sin(Time.time * _sineSpeed);
        sine = (sine + 1) / 2.0f;
        sine *= _sineMagnitude;
        _itemVisual.transform.localPosition = new Vector3(_defaultVisualPos.x, _visualStartY + sine, _defaultVisualPos.z);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.GetComponent<PlayerMovementController>() != null)
        {
            AudioManager.PlaySound(_pickupClip);
            
            if(_fpsWeaponPrefab != null)
                PlayerWeaponController.PickupWeapon(this);
    
            Destroy(gameObject);
        }
    }

    public GameObject GetPrefab()
    {
        return _fpsWeaponPrefab;
    }
}