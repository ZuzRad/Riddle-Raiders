using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffMusic : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;
    private UnityEngine.UI.Button toggleButton;
    private bool isMuted = false;
    private float previousVolume;
    private Image buttonImage;

    private void Start()
    {
        toggleButton = GetComponent<UnityEngine.UI.Button>();
        toggleButton.onClick.AddListener(ToggleMusic);
        buttonImage = toggleButton.GetComponent<Image>();
        if (GlobalVolumeManager.GetMusicVolume() == 0)
        {
            isMuted = true;
            buttonImage.sprite = onSprite;
        }
        else
        {
            isMuted = false;
            buttonImage.sprite = offSprite;
        }
    }

    private void ToggleMusic()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            previousVolume = GlobalVolumeManager.GetMusicVolume();
            GlobalVolumeManager.SetMusicVolume(0f);
            musicSlider.value = 0f;
            buttonImage.sprite = onSprite;
            musicSlider.interactable = false;
        }
        else
        {
            GlobalVolumeManager.SetMusicVolume(previousVolume);
            musicSlider.value = previousVolume;
            buttonImage.sprite = offSprite;
            musicSlider.interactable = true;
        }
    }
}
