using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelStatsItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI completionTimeText;
    [SerializeField] private CollectedStarsDisplayer starsCollectedDisplayer;

    public void SetItemInfo(string completionTime, int collectedStars)
    {
        starsCollectedDisplayer.SetCollectedStars(collectedStars);
        completionTimeText.text = completionTime;
    }
}
