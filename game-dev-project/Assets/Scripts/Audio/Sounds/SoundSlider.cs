using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    public Action onChangeSoundVolume;
    private void Start()
    {
        Slider volumeSlider = GetComponent<Slider>();
        volumeSlider.value = GlobalVolumeManager.GetSoundsVolume();
        if (volumeSlider.value == 0)
        {
            volumeSlider.interactable = false;
        }
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float volume)
    {
        GlobalVolumeManager.SetSoundsVolume(volume);
        onChangeSoundVolume?.Invoke();
    }
}
