using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void PauseGame(bool state);
    public static event PauseGame OnGamePaused;
    private static GameManager instance;

    private bool _isGamePaused = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isGamePaused = !_isGamePaused;
            OnGamePaused?.Invoke(_isGamePaused);
            
            Debug.Log($"Paused: {_isGamePaused}");
        }   
    }
}
