using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedStarsDisplayer : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite colletedStar;
    [SerializeField] private Sprite missingStar;

    private void Awake()
    {
        ResetStars();
    }

    public void ResetStars()
    {
        foreach (var star in stars)
        {
            star.sprite = missingStar;
        }
    }
    
    public void SetCollectedStars(int number)
    {
        for (int i = 0; i < number; i++)
        {
            stars[i].sprite = colletedStar;
        }
    }
}
