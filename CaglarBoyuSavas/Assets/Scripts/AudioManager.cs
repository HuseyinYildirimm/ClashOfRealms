using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    private static AudioManager _Instance;

    public static AudioManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<AudioManager>();
            }
            return _Instance;
        }
    }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Start()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = false;
            sound.source.loop = sound.loop;
        }

        Instance.Play("Soundtrack1");
    }

    public void Play(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);

        s.source.Play();
    }

    public void Stop(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == audioName);

        s.source.Stop();
    }
}
