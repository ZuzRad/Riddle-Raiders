using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionController : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;
    [SerializeField] private LevelItemGridGenerator levelItemGridGenerator;
    [SerializeField] private UnityEngine.UI.Button backToMenuButton;
    [SerializeField] private UnityEngine.UI.Button resetDataButton;
    PlayerGameProgress playerGameProgress;
    
    private void Awake() 
    {
        playerGameProgress = dataManager.LoadData();
        levelItemGridGenerator.GenerateLevelItems(playerGameProgress.LevelsData);
        levelItemGridGenerator.onSelectLevel += HandleLevelSelect;
        
        backToMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });
        
        resetDataButton.onClick.AddListener(() =>
        {
            playerGameProgress = dataManager.ResetData();
            levelItemGridGenerator.GenerateLevelItems(playerGameProgress.LevelsData);
        });
    }

    private void OnDestroy()
    {
        levelItemGridGenerator.onSelectLevel -= HandleLevelSelect;
    }

    private void HandleLevelSelect(int levelNumber) 
    {
        if (levelNumber + 1 >= 0)
        {
            dataManager.CurrentLvl = levelNumber + 1;
            SceneManager.LoadScene(playerGameProgress.LevelsData[levelNumber].PathToScene);
        }
    }
}
