
using System;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplayer : MonoBehaviour
{
    [SerializeField] private Image starImage;
    [SerializeField] private Sprite[] stars;

    int currentStarAmount = 0;
    
    private void Start() 
    {
        starImage.sprite = stars[currentStarAmount];
    }

    public void ChangeStarsNumber() 
    {
        starImage.sprite = stars[++currentStarAmount];
    }
}
