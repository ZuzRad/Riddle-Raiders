using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioClip jump, endLevel, collect, hit, wind, box, slide, climb, laser, nextTime;

    AudioSource audioSource;
    Dictionary<Sounds, AudioClip> sounds;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GlobalVolumeManager.GetSoundsVolume();
        sounds = new Dictionary<Sounds, AudioClip>
        {
            { Sounds.Jump, jump },
            { Sounds.EndLevel, endLevel },
            { Sounds.Collect, collect },
            { Sounds.Hit, hit },
            { Sounds.Wind, wind },
            { Sounds.Box, box },
            { Sounds.Slide, slide },
            { Sounds.Climb, climb },
            { Sounds.Laser, laser },
            { Sounds.NextTime, nextTime }
        };
        
        BindAllEnvironmentElements();
    }
    
    public void PlaySound(Sounds sound)
    {
        audioSource.volume = GlobalVolumeManager.GetSoundsVolume();
        audioSource.PlayOneShot(sounds[sound]);
    }

    public void SetClip(Sounds sound)
    {
        audioSource.volume = GlobalVolumeManager.GetSoundsVolume();
        audioSource.clip = sounds[sound];
        
        if (!audioSource.isPlaying) 
        {
            audioSource.Play();
        }
    }

    public void StopPlaying()
    {
        audioSource.Stop();
    }
    
    public void SetLoop(bool isLooped)
    {
        audioSource.loop = isLooped;
    }
    
    private void BindAllEnvironmentElements()
    {
        Geyser[] geysers = FindObjectsOfType<Geyser>(true);
        foreach (var geyser in geysers) {
            geyser.onGeyserActivation += () => PlaySound(Sounds.Wind);
        }

        SoundSlider[] sliders = FindObjectsOfType<SoundSlider>();
        foreach (var slider in sliders) {
            slider.onChangeSoundVolume += () => PlaySound(Sounds.Collect);
        }

        LaserReciever[] lasers = FindObjectsOfType<LaserReciever>();
        foreach (var laser in lasers)
        {
            laser.onLaserActivation += () => PlaySound(Sounds.Laser);
        }
    }
    
    public enum Sounds
    {
        Jump, EndLevel, Collect, Hit, Wind, Box, Slide, Climb, Laser, NextTime
    }
}
