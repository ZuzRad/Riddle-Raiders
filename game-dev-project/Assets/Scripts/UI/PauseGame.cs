using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public Action onBackButtonClicked;
    public Action onResetLevel;
    public Action onResumeLevel;
    [SerializeField] private UnityEngine.UI.Button backButton;
    [SerializeField] private UnityEngine.UI.Button resetLevelButton;
    [SerializeField] private UnityEngine.UI.Button resumeLevelButton;

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

        resumeLevelButton.onClick.AddListener(() =>
        {
            onResumeLevel?.Invoke();
        });
    }
}
