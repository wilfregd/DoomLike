using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerFootstepsController : MonoBehaviour
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private float _delay = 0.7f;
    [SerializeField] private float _sprintDelay = 0.4f;
    private PlayerMovementController _controller;
    private AudioSource _source;
    private List<AudioClip> _currentClips = new List<AudioClip>();
    private AudioClip _lastClip;
    private float _currentElapsedTime;
    
    private void Awake()
    {
        _controller = GetComponent<PlayerMovementController>();
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        RandomizeList();
    }

    private void Update()
    {
        _currentElapsedTime += Time.deltaTime;
        if ((!_controller.isSprinting && _currentElapsedTime >= _delay) || (_controller.isSprinting && _currentElapsedTime >= _sprintDelay))
        {
            if (_controller.movementMagnitude != 0)
            {
                if (_currentClips.Count == 0)
                {
                    RandomizeList();
                }
                
                _lastClip = _currentClips[0];
                _source.PlayOneShot(_currentClips[0]);
                _currentClips.RemoveAt(0);
            }
            _currentElapsedTime = 0.0f;
        }
    }

    private void RandomizeList()
    {
        _currentClips = new List<AudioClip>(_clips);
        for (int i = 0; i < _currentClips.Count; i++)
        {
            AudioClip temp = _currentClips[i];
            int randomIndex = Random.Range(i, _currentClips.Count);
            _currentClips[i] = _currentClips[randomIndex];
            _currentClips[randomIndex] = temp;
        }

        if (_lastClip == _currentClips[0])
        {
            AudioClip c = _currentClips[0];
            _currentClips.RemoveAt(0);
            _currentClips.Add(c);
        }
    }
}
