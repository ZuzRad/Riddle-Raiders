using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    public Action onPanelClose;
    [Header("Panels")] 
    [SerializeField] private List<GameObject> panels;

    [Header("Button")]
    [SerializeField] private UnityEngine.UI.Button leftButton;
    [SerializeField] private UnityEngine.UI.Button rightButton;
    [SerializeField] private UnityEngine.UI.Button closeButton;

    int currentIndexPanel = 0;
    
    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            onPanelClose?.Invoke();
        });
        leftButton.onClick.AddListener(SetActivePreviousPanel);
        rightButton.onClick.AddListener(SetActiveNextPanel);
        SetActivePanel(currentIndexPanel);
        leftButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        leftButton.onClick.RemoveListener(SetActivePreviousPanel);
        rightButton.onClick.RemoveListener(SetActiveNextPanel);
    }

    private void SetActiveNextPanel()
    {
        leftButton.gameObject.SetActive(true);
        SetActivePanel(++currentIndexPanel);
        
        if (currentIndexPanel + 1 >= panels.Count) 
        {
            rightButton.gameObject.SetActive(false);
        }
    }

    private void SetActivePreviousPanel()
    {
        rightButton.gameObject.SetActive(true);
        SetActivePanel(--currentIndexPanel);
        
        if (currentIndexPanel - 1 < 0) 
        {
            leftButton.gameObject.SetActive(false);
        }
    }

    private void SetActivePanel(int index)
    {
        SetActivePanels(false);
        panels[index].SetActive(true);
    }
    
    private void SetActivePanels(bool isActive) {
        panels.ForEach(panel => panel.SetActive(isActive));
    }
}
