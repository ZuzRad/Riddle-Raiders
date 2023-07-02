using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObjects/DataManager", order = 1)]
public class DataManager : ScriptableObject {
    public int CurrentLvl { get; set; }
    [SerializeField] private string[] levelPaths;
    private string PLAYER_DATA_KEY = "playerLevelProgress";

    public void SaveData(Explanation explanation, int starsAmount, int levelNumber, float completionTime)
    {
        PlayerGameProgress playerGameProgress = LoadData();
        playerGameProgress.ChangeStarsNumber(starsAmount, levelNumber);
        playerGameProgress.ChangeCompletionTime(explanation, completionTime, levelNumber);
        
        if (levelNumber + 1 < playerGameProgress.LevelsData.Count)
        {
            playerGameProgress.UnlockLevel(levelNumber + 1);
        }
        
        SaveDataToPlayerPrefs(playerGameProgress);
    }


    public PlayerGameProgress LoadData()
    {
        PlayerGameProgress playerGameProgress;
        
        if (PlayerPrefs.HasKey(PLAYER_DATA_KEY))
        {
            playerGameProgress = JsonUtility.FromJson<PlayerGameProgress>(PlayerPrefs.GetString(PLAYER_DATA_KEY));
        }
        else
        {
            playerGameProgress = ResetData();
        }

        return playerGameProgress;
    }
    
    public PlayerGameProgress ResetData()
    {
        PlayerGameProgress newData = new PlayerGameProgress();
        newData.InitLevels(levelPaths);
        SaveDataToPlayerPrefs(newData);
        return newData;
    }

    void SaveDataToPlayerPrefs(PlayerGameProgress playerGameProgress) 
    {
        string json = JsonUtility.ToJson(playerGameProgress);
        PlayerPrefs.SetString(PLAYER_DATA_KEY, json);
    }
}
