using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class PlayerGameProgress
{
    public List<LevelData> LevelsData => levelsData;
    [SerializeField] private List<LevelData> levelsData;
    
    public PlayerGameProgress()
    {
        levelsData = new List<LevelData>();
    }
    
    public PlayerGameProgress(List<LevelData> levelsData)
    {
        this.levelsData = levelsData;
    }
    
    public void InitLevels(string[] levelPaths)
    {
        levelsData = new List<LevelData>();
        for (int i = 0; i < levelPaths.Length; i++)
        {
            levelsData.Add(new LevelData(levelPaths[i], i, 0, i != 0));
        }
    }

    public void ChangeStarsNumber(int starsAmount, int levelNumber) {
        LevelData levelData = levelsData[levelNumber - 1];
        int lastStarsAmount = levelData.StarsAmount;
        
        if (lastStarsAmount <= starsAmount)
        {
            levelsData[levelNumber - 1].StarsAmount = starsAmount;
        }
    }

    public void UnlockLevel(int levelNumber)
    {
        levelsData[levelNumber - 1].UnlockLevel();
    }

    public void ChangeCompletionTime(Explanation explanation, float completionTime, int levelNumber)
    {
        LevelData levelData = levelsData[levelNumber - 1];
        
        if (explanation == Explanation.BetterTimeEqualStars || 
            explanation == Explanation.BetterTimeMoreStars|| 
            explanation == Explanation.WorseTimeMoreStars) {
            levelData.CompletionTime = completionTime;
        }
    }
}
