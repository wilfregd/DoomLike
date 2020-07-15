using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource _source;

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

        _source = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        instance.Play(clip);
    }

    protected void Play(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
