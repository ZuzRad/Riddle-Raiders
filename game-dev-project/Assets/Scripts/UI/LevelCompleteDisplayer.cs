using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Explanation
{
    WorseTimeMoreStars,
    BetterTimeEqualStars,
    WorseTimeEqualStars,
    BetterTimeLessStars,
    WorseTimeLessStars,
    BetterTimeMoreStars
}

public class LevelCompleteDisplayer : MonoBehaviour
{
    public Action onBackButtonClicked;
    public Action onResetButtonClicked;
    public Action onNextLevelButtonClicked;
    
    [Header("Score prompt")]
    [SerializeField] private TextMeshProUGUI headerPromptText;
    [SerializeField] private TextMeshProUGUI explanationPromptText;

    [SerializeField] private LevelStatsItem lastTimeStats;
    [SerializeField] private LevelStatsItem currentTimeStats;
    
    [Header("Buttons")]
    [SerializeField] private UnityEngine.UI.Button backButton;
    [SerializeField] private UnityEngine.UI.Button resetLevelButton;
    [SerializeField] private UnityEngine.UI.Button nextLevelButton;
    
    Dictionary<Explanation, string[]> levelCompletePrompts;
    
    private void Awake()
    {
        levelCompletePrompts = new Dictionary<Explanation, string[]>
        {
            { Explanation.WorseTimeMoreStars, new[] { "NEW HIGHSCORE!", "You achieved more stars!" }},
            { Explanation.BetterTimeEqualStars, new[] { "NEW HIGHSCORE!", "You beat completion time!" }},
            { Explanation.BetterTimeMoreStars, new[] { "NEW HIGHSCORE!", "You beat completion time and achieved more stars!" }},
            { Explanation.WorseTimeEqualStars, new[] { "MAYBE NEXT TIME!", "You achieved the same amount of stars " +
                                                                           "and your completion time is worse!" }},      
            { Explanation.BetterTimeLessStars, new[] { "MAYBE NEXT TIME!", "Your completion time is better, " +
                                                                           "but you achieved less stars this time!" }},
            { Explanation.WorseTimeLessStars, new[] { "MAYBE NEXT TIME!", "You achieved less stars " +
                                                                          "and your completion time is worse!" }},
        };
        
        backButton.onClick.AddListener(() =>
        {
            onBackButtonClicked?.Invoke();
        });        
        
        resetLevelButton.onClick.AddListener(() =>
        {
            onResetButtonClicked?.Invoke();
        });
        
        nextLevelButton.onClick.AddListener(() =>
        {
            onNextLevelButtonClicked?.Invoke();
        });
    }

    public void ShowLevelCompletionPanel(Explanation explanation, 
                                         int lastCollectedStars, 
                                         int currentCollectedStars, 
                                         string lastCompletionTime, 
                                         string currentCompletionTime)
    {
        string[] promptTexts = levelCompletePrompts[explanation];
        headerPromptText.text = promptTexts[0];
        explanationPromptText.text = promptTexts[1];
        
        lastTimeStats.SetItemInfo(lastCompletionTime,lastCollectedStars);
        currentTimeStats.SetItemInfo(currentCompletionTime,currentCollectedStars);
    }

    public void HandleGameComplete()
    {
        nextLevelButton.gameObject.SetActive(false);
    }
}
