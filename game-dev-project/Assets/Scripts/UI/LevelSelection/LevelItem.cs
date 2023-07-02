using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour 
{
    public Action<LevelItem> onLevelClicked;
    public int LevelNumber => levelNumber;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private TextMeshProUGUI completionText;
    [SerializeField] private CollectedStarsDisplayer starsDisplayer;
    [SerializeField] private Image lockedLevelCover;
    [SerializeField] private UnityEngine.UI.Button button;

    bool isLevelLocked = false;
    int levelNumber; 
    
    private void Awake()
    {
        button.onClick.AddListener(HandleButtonClicked);
        starsDisplayer.ResetStars();
    }


    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleButtonClicked);
    }
    
    public void SetData(int starsAmount, int levelNumber, bool isLocked, string completionTime )
    {
        levelNumberText.text = (levelNumber + 1).ToString();
        starsDisplayer.SetCollectedStars(starsAmount);
        isLevelLocked = isLocked;
        this.levelNumber = levelNumber;
        completionText.text = completionTime;
        
        if (isLevelLocked)
        {
            lockedLevelCover.gameObject.SetActive(true);
            button.enabled = false;
        }
    }
    
    private void HandleButtonClicked()
    {
        onLevelClicked?.Invoke(this);
    }

}
