using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffSounds : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
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
        if (GlobalVolumeManager.GetSoundsVolume() == 0)
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
            previousVolume = GlobalVolumeManager.GetSoundsVolume();
            GlobalVolumeManager.SetSoundsVolume(0f);
            soundSlider.value = 0f;
            buttonImage.sprite = onSprite;
            soundSlider.interactable = false;
        }
        else
        {
            GlobalVolumeManager.SetSoundsVolume(previousVolume);
            soundSlider.value = previousVolume;
            buttonImage.sprite = offSprite;
            soundSlider.interactable = true;
        }
    }
}
