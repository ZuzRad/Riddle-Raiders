using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Bson;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Character bigPlayer;
    [SerializeField] private Character smallPlayer;

    [Header("Items")] 
    [SerializeField] private StarCollectDetector starCollectDetector;
    
    [Header("UI objects")] 
    [SerializeField] private UIController uiController;

    [Header("End level")]
    [SerializeField] private EnterEndDoor endDoor;
    
    [Header("Sound Managers")]
    [SerializeField] private GameMusicManager gameMusicManager;
    [SerializeField] private SoundManager soundManager;
    
    [Header("Data Manager")]
    [SerializeField] private DataManager dataManager;
    [SerializeField] private Timer timer;
    PlayerGameProgress playerGameProgress;

    [Header("Effects")]
    [SerializeField] private GameObject fireworkEffect;
    [SerializeField] private GameObject fireworkShootEffect;
    [SerializeField] private GameObject smokeEffect;

    private const string player1KeyBindingsConfKey = "PLAYER_1_BINDINGS";
    private const string player2KeyBindingsConfKey = "PLAYER_2_BINDINGS";
    private const string player1ActionMap = "Player1";
    private const string player2ActionMap = "Player2";
    
    private void Start() 
    {
        BindToEvents();
        playerGameProgress = dataManager.LoadData();
        uiController.SetLevelNumber(dataManager.CurrentLvl);
        uiController.onCloseTutorial += HandleCloseTutorialPanel;
        
        if (dataManager.CurrentLvl == 1)
        {
            HandleOpenTutorialPanel();
        }
        
        LoadKeyBindings(player1KeyBindingsConfKey, smallPlayer, player1ActionMap);
        LoadKeyBindings(player2KeyBindingsConfKey, bigPlayer, player2ActionMap);

        GameMusicManager.Music randomMusic = (GameMusicManager.Music)UnityEngine.Random.Range(0, 3);
        gameMusicManager.PlayMusic(randomMusic);
    }

    private void HandleCloseTutorialPanel()
    {
        timer.ResetTimer();
        timer.IsCounting = true;
        bigPlayer.Movement.ForceStartPlayer();
        smallPlayer.Movement.ForceStartPlayer();
        uiController.onCloseTutorial -= HandleCloseTutorialPanel;
    }

    private void HandleOpenTutorialPanel()
    {
        timer.IsCounting = false;
        uiController.OpenTutorialPanel();
        bigPlayer.Movement.ForceStopPlayer();
        smallPlayer.Movement.ForceStopPlayer();
    }

    private void OnDestroy()
    {
        UnbindFromEvents();
    }

    private void BindToEvents()
    {
        bigPlayer.onPlayersDeath += HandleGameOverState;
        bigPlayer.onPlayerHit += HandlePlayerHit;
        
        smallPlayer.onPlayersDeath += HandleGameOverState;
        smallPlayer.onPlayerHit += HandlePlayerHit;

        starCollectDetector.onStarCollected += HandleStarCollecting;

        endDoor.onEnterEndDoor += HandleEndLevel;

        uiController.onBackToMenu += HandleBackToMenu;
        uiController.onNextLevelButtonClicked += HandleNextLevelButtonClicked;
        uiController.onResetLevel += HandleResetLevel;

        uiController.onResumeLevel += HandleResume;

        smallPlayer.Movement.onEscPressed += HandlePauseGame;

        timer.onTimeTick += HandleTimeTick;
    }

    private void HandleTimeTick(float currentTime)
    {
        uiController.UpdateTime(GetFormatedTime(currentTime));
    }

    private string GetFormatedTime(float time)
    {
        if (time == -1) 
        {
            return "--:--";
        }
        
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        
        StringBuilder builder = new StringBuilder();
        builder.Append(minutes < 10 ? "0" : "");
        builder.Append(minutes);
        builder.Append(":");
        builder.Append(seconds < 10 ? "0" : "");
        builder.Append(seconds);
        return builder.ToString();
    }
    
    private void HandleResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void HandleResume()
    {
        timer.IsCounting = true;
        uiController.SetActivePausePanel(false);
        bigPlayer.Movement.ForceStartPlayer();
        smallPlayer.Movement.ForceStartPlayer();
    }

    private void HandleNextLevelButtonClicked()
    {
        dataManager.CurrentLvl++;
        int levelIndex = dataManager.CurrentLvl - 1;
        
        if (levelIndex >= 0) {
            SceneManager.LoadScene(playerGameProgress.LevelsData[levelIndex].PathToScene);
        }
    }

    private void HandleStarCollecting()
    {
        soundManager.PlaySound(SoundManager.Sounds.Collect);
        uiController.IncreaseStarAmount();
    }

    private void UnbindFromEvents()
    {
        bigPlayer.onPlayersDeath -= HandleGameOverState;
        bigPlayer.onPlayerHit -= HandlePlayerHit;
        
        smallPlayer.onPlayersDeath -= HandleGameOverState;
        smallPlayer.onPlayerHit -= HandlePlayerHit;
        
        starCollectDetector.onStarCollected -= HandleStarCollecting;
        
        endDoor.onEnterEndDoor -= HandleEndLevel;
        uiController.onResumeLevel -= HandleResume;
        uiController.onBackToMenu -= HandleBackToMenu;
        uiController.onNextLevelButtonClicked -= HandleNextLevelButtonClicked;
        smallPlayer.Movement.onEscPressed -= HandlePauseGame;
        
        timer.onTimeTick += HandleTimeTick;
    }

    private void HandleEndLevel()
    {
        timer.IsCounting = false;
        Explanation explanation = OpenLevelCompletePanel();
        dataManager.SaveData(explanation, starCollectDetector.StarsAmount, dataManager.CurrentLvl, timer.CurrentTime);


        if (explanation == Explanation.WorseTimeEqualStars || explanation == Explanation.BetterTimeLessStars || explanation == Explanation.WorseTimeLessStars)
        {
            soundManager.PlaySound(SoundManager.Sounds.NextTime);
            Instantiate(smokeEffect, endDoor.transform.position + new Vector3(2, 0, 0), Quaternion.identity);
        }
        else
        {
            soundManager.PlaySound(SoundManager.Sounds.EndLevel);
            Instantiate(fireworkEffect, endDoor.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
            Instantiate(fireworkShootEffect, endDoor.transform.position + new Vector3(1, 0, 0), Quaternion.identity);
        }

        if (dataManager.CurrentLvl == playerGameProgress.LevelsData.Count)
        {
            uiController.HandleGameComplete();
        }
    }

    private Explanation OpenLevelCompletePanel()
    {
        int levelIndex = dataManager.CurrentLvl - 1;
        LevelData currentLevel = playerGameProgress.LevelsData[levelIndex];
        int lastCollectedStars = currentLevel.StarsAmount;
        int currentCollectedStars = starCollectDetector.StarsAmount;
        float lastCompletionTime = currentLevel.CompletionTime;
        float currentCompletionTime = timer.CurrentTime;
        
        Explanation explanation;
        if (lastCollectedStars < currentCollectedStars)
        {
            explanation = lastCompletionTime == -1 || lastCompletionTime > currentCompletionTime ? 
                Explanation.BetterTimeMoreStars : Explanation.WorseTimeMoreStars;
        }
        else if (lastCollectedStars == currentCollectedStars)
        {
            explanation = lastCompletionTime == -1 || lastCompletionTime > currentCompletionTime ? 
                Explanation.BetterTimeEqualStars : Explanation.WorseTimeEqualStars;
        }
        else
        {
            explanation = lastCompletionTime == -1 || lastCompletionTime > currentCompletionTime ? 
                Explanation.BetterTimeLessStars : Explanation.WorseTimeLessStars;
        }
        
        uiController.OpenLevelCompletePanel(explanation, 
                                            lastCollectedStars,
                                            currentCollectedStars,
                                            GetFormatedTime(lastCompletionTime),
                                            GetFormatedTime(currentCompletionTime));
        return explanation;
    }

    private void HandlePlayerHit(Character character)
    {
        soundManager.PlaySound(SoundManager.Sounds.Hit);
        uiController.DecreasePlayersHealth(character.Type);
    }
    
    private void HandleGameOverState()
    {
        timer.IsCounting = false;
        uiController.SetActiveGameOverPanel(true);
        bigPlayer.Movement.ForceStopPlayer();
        smallPlayer.Movement.ForceStopPlayer();
    }

    private void HandlePauseGame()
    {
        timer.IsCounting = false;
        uiController.SetActivePausePanel(true);
        bigPlayer.Movement.ForceStopPlayer();
        smallPlayer.Movement.ForceStopPlayer();
    }

    private void LoadKeyBindings(string confKey, Character character, string actionMap)
    {
        string rebinds = PlayerPrefs.GetString(confKey, string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            character.Movement.PlayerInput.actions.FindActionMap(actionMap).LoadBindingOverridesFromJson(rebinds);
        }
    }
}
