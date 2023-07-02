using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action onNextLevelButtonClicked;
    public Action onBackToMenu;
    public Action onResetLevel;
    public Action onResumeLevel;
    public Action onCloseTutorial;
    [Header("UI objects")] 
    
    [Header("Top info")]
    [SerializeField] private PlayerStateController smallPlayerStateController;
    [SerializeField] private PlayerStateController bigPlayerStateController;
    [SerializeField] private StarDisplayer starDisplayer;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    
    [Header("Panels")]
    [SerializeField] private LevelCompleteDisplayer levelCompleteDisplayer;
    [SerializeField] private GameOverPanelDisplayer gameOverPanelDisplayer;
    [SerializeField] private PauseGame pausePanelDisplayer;
    [SerializeField] private TutorialPanelController tutorialPanelController;
    private void Start()
    {
        levelCompleteDisplayer.gameObject.SetActive(false);
        gameOverPanelDisplayer.gameObject.SetActive(false);
        tutorialPanelController.gameObject.SetActive(false);
        SetActiveGameOverPanel(false);
        SetActivePausePanel(false);
        levelCompleteDisplayer.onNextLevelButtonClicked += () => { onNextLevelButtonClicked?.Invoke(); };
        levelCompleteDisplayer.onBackButtonClicked += () => { onBackToMenu?.Invoke(); };
        levelCompleteDisplayer.onResetButtonClicked += () => { onResetLevel?.Invoke(); };
        gameOverPanelDisplayer.onResetLevel += () => { onResetLevel?.Invoke(); };
        gameOverPanelDisplayer.onBackButtonClicked += () => { onBackToMenu?.Invoke(); };
        pausePanelDisplayer.onResetLevel += () => { onResetLevel?.Invoke(); };
        pausePanelDisplayer.onBackButtonClicked += () => { onBackToMenu?.Invoke(); };
        pausePanelDisplayer.onResumeLevel += () => { onResumeLevel?.Invoke(); };
        tutorialPanelController.onPanelClose += () => { onCloseTutorial?.Invoke(); };
    }

    public void UpdateTime(string time)
    {
        timeText.text = time;
    }
    
    public void DecreasePlayersHealth(Character.CharacterType characterType)
    {
        switch (characterType)
        {
            case Character.CharacterType.Big:
                bigPlayerStateController.DecreaseHeartsAmount();
                break;
            case Character.CharacterType.Small:
                smallPlayerStateController.DecreaseHeartsAmount();
                break;
        }
    }

    public void OpenTutorialPanel()
    {
        tutorialPanelController.gameObject.SetActive(true);
    }
    
    public void SetActiveGameOverPanel(bool isActive)
    {
        gameOverPanelDisplayer.gameObject.SetActive(isActive);
    }

    public void SetActivePausePanel(bool isActive)
    {
        pausePanelDisplayer.gameObject.SetActive(isActive);
    }

    public void OpenLevelCompletePanel(Explanation explanation,
                                       int lastCollectedStars, 
                                       int collectedStars, 
                                       string lastCompletionTime, 
                                       string currentCompletionTime)
    {
        levelCompleteDisplayer.gameObject.SetActive(true);
        levelCompleteDisplayer.ShowLevelCompletionPanel(explanation, 
                                                        lastCollectedStars, 
                                                        collectedStars,
                                                        lastCompletionTime, 
                                                        currentCompletionTime);
    }

    public void IncreaseStarAmount()
    {
        starDisplayer.ChangeStarsNumber();
    }

    public void HandleGameComplete()
    {
        levelCompleteDisplayer.HandleGameComplete();
    }

    public void SetLevelNumber(int levelNumber)
    {
        levelText.text = "Level " + levelNumber;
    }
}
