using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelItemGridGenerator : MonoBehaviour
{
    public Action<int> onSelectLevel;
    
    [SerializeField] private LevelItem levelItemPrefab;
    public void GenerateLevelItems(List<LevelData> levelsData)
    {
        LevelItem[] childObjects = GetComponentsInChildren<LevelItem>(false);
        childObjects.ToList().ForEach(childObject => Destroy(childObject.gameObject));
        
        foreach (var levelData in levelsData)
        {
            LevelItem newLevelItem = Instantiate(levelItemPrefab);
            newLevelItem.transform.SetParent(gameObject.transform, false);
            newLevelItem.SetData(levelData.StarsAmount, levelData.LevelNumber, levelData.IsLocked, GetFormatedTimeText(levelData.CompletionTime));
            newLevelItem.onLevelClicked += (levelItem) =>
            {
                onSelectLevel?.Invoke(levelItem.LevelNumber);
            };
        }
    }

    string GetFormatedTimeText(float completionTime)
    {
        if (completionTime == -1) {
            return "--:--";
        }
        
        int minutes = Mathf.FloorToInt(completionTime / 60);
        int seconds = Mathf.FloorToInt(completionTime % 60);
        
        StringBuilder builder = new StringBuilder();
        builder.Append(minutes < 10 ? "0" : "");
        builder.Append(minutes);
        builder.Append(":");
        builder.Append(seconds < 10 ? "0" : "");
        builder.Append(seconds);

        return builder.ToString();
    }
}
