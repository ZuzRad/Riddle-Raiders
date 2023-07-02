using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolumeManager : MonoBehaviour
{
    private static float musicVolume = 0.5f;
    private static float soundsVolume = 0.5f;

    public static float GetMusicVolume()
    {
        return musicVolume;
    }

    public static void SetMusicVolume(float volume)
    {
        musicVolume = volume;
    }

    public static float GetSoundsVolume()
    {
        return soundsVolume;
    }

    public static void SetSoundsVolume(float volume)
    {
        soundsVolume = volume;
    }
}
