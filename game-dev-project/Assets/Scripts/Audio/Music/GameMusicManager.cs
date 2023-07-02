using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour
{
    public enum Music
    {
        GameMusic1, GameMusic2, GameMusic3
    }

    [Header("Music clips")]
    [SerializeField] private AudioClip gameMusic1, gameMusic2, gameMusic3;
    
    [SerializeField] AudioSource audioSource;

    Dictionary<Music, AudioClip> musicClips;

    private void Awake()
    {
        audioSource.volume = GlobalVolumeManager.GetMusicVolume();

        musicClips = new Dictionary<Music, AudioClip> {
            { Music.GameMusic1, gameMusic1 },
            { Music.GameMusic2, gameMusic2 },
            { Music.GameMusic3, gameMusic3 }
        };
    }

    public void PlayMusic(Music music)
    {
        audioSource.clip = musicClips[music];
        audioSource.loop = true;
        audioSource.Play();
    }
}