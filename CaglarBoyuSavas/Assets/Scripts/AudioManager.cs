using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    UIManager uiManager;
    public Sound[] sounds;
    private List<AudioSource> audioSourceList = new List<AudioSource>();
    private List<float> originalVolumes = new List<float>();
    private static AudioManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();

        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = false;
            sound.source.loop = sound.loop;

            sound.volume = uiManager.SfxSlider.value;

            audioSourceList.Add(sound.source);
            
        }


        foreach (var audioSource in audioSourceList)
        {
            originalVolumes.Add(audioSource.volume);
        }
    }

    public void AdjustAllVolumes(float volume)
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            float originalVolume = originalVolumes[i];
            float newVolume = originalVolume * volume;

            audioSourceList[i].volume = newVolume;
        }
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
