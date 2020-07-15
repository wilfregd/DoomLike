using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorSliding : MonoBehaviour, IInteractive
{
    [SerializeField] private Vector3 _lerpOffset;
    [SerializeField] private float _returnDelay = 5.0f;
    [SerializeField] private AudioClip _openingSound;
    [SerializeField] private BoxCollider _crushZoneCollider;
    [SerializeField, Range(0.0f, 100.0f)] private float _crushOffset;
    [SerializeField] private LayerMask _crushMask;
    private bool _isMoving;
    private bool _isOpened;
    private Vector3 _originPos;
    private AudioSource _source;

    private void Awake()
    {
        _originPos = transform.position;
        _source = GetComponent<AudioSource>();
    }

    public void OnInteract()
    {
        if (!_isMoving && !_isOpened)
        {
            StartCoroutine(Translate(true));
            Invoke(nameof(Recover), _returnDelay);
        }
    }

    private IEnumerator Translate(bool open)
    {
        _isMoving = true;
        float elapsedTime = 0.0f;
        float waitTime = 0.25f;
        Vector3 destinationPos = open ? _originPos + _lerpOffset : _originPos;

        _source.PlayOneShot(_openingSound);
        
        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position, destinationPos, elapsedTime / waitTime);
            elapsedTime += Time.deltaTime / 2.0f;
            
            if (_isOpened && ((elapsedTime / waitTime) * 100) >= _crushOffset)
            {
                TryCrush();
            }
            
            yield return null;
        }

        transform.position = destinationPos;

        _isOpened = open;
        
        _isMoving = false;
        
    }

    private void TryCrush()
    {
        var colliders = Physics.OverlapBox(_crushZoneCollider.transform.TransformPoint(_crushZoneCollider.center), _crushZoneCollider.transform.TransformVector(_crushZoneCollider.size * 0.5f), _crushZoneCollider.transform.rotation, _crushMask);

        if (colliders != null && colliders.Length > 0)
        {
            Debug.LogWarning("GOT PLAYER!");
            foreach (var c in colliders)
            {
                PlayerMovementController player = c.GetComponent<PlayerMovementController>();
                if (player != null)
                {
                    player.ForceKillPlayer();
                    return;
                }
            }
        }
    }
    
    private void Recover()
    {
        StartCoroutine(Translate(false));
    }
}
