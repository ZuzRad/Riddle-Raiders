using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusicManager : MonoBehaviour
{

    [SerializeField] private AudioClip menuMusic;

    AudioSource audioSource;
    private Scene activeScene;
    private static MenuMusicManager menuMusicManagerInctance;
    private void Awake()
    {
        if (menuMusicManagerInctance == null)
        {
            menuMusicManagerInctance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GlobalVolumeManager.GetMusicVolume();
    }
    private void Update()
    {
        activeScene = SceneManager.GetActiveScene();
        if (activeScene.name.StartsWith("Level_"))
        {
            Destroy(gameObject);
        }
        audioSource.volume = GlobalVolumeManager.GetMusicVolume();
    }

    private void OnEnable()
    {
        MenuPanelController menuPanelController = FindObjectOfType<MenuPanelController>();
        if (menuPanelController != null)
        {
            menuPanelController.MenuMusic += PlayMusic;
        }
    }

    private void OnDisable()
    {
        MenuPanelController menuPanelController = FindObjectOfType<MenuPanelController>();
        if (menuPanelController != null)
        {
            menuPanelController.MenuMusic -= PlayMusic;
        }
    }

    public void PlayMusic()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = menuMusic;
        audioSource.loop = true;
        audioSource.Play();

    }
}
