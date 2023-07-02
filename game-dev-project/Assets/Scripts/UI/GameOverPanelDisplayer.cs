using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameOverPanelDisplayer : MonoBehaviour
{
    public Action onBackButtonClicked;
    public Action onResetLevel;
    [SerializeField] private UnityEngine.UI.Button backButton;
    [SerializeField] private UnityEngine.UI.Button resetLevelButton;

    private void Start() 
    {
        backButton.onClick.AddListener(() =>
        {
            onBackButtonClicked?.Invoke();
        });
        
        resetLevelButton.onClick.AddListener(() =>
        {
            onResetLevel?.Invoke();
        });
    }
}
