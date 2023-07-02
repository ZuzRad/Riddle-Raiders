using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyItem : MonoBehaviour {
    [SerializeField] private InputActionReference action;
    [SerializeField] private bool isComposite;
    [SerializeField] private string name;

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Button bindButton;
    [SerializeField] private TextMeshProUGUI bindingDisplayNameText;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        bindButton.onClick.AddListener(RebindKey);
        RefreshContent();
    }

    private void OnDestroy()
    {
        bindButton.onClick.RemoveListener(RebindKey);
    }

    public void RebindKey()
    {
        bindButton.gameObject.SetActive(false);
        if (isComposite && name != String.Empty) {
            var bindingIndex = action.action.bindings.IndexOf(x => x.isPartOfComposite && x.name == name);
            rebindingOperation = action.action.PerformInteractiveRebinding().WithTargetBinding(bindingIndex);
        }
        else
        {
            rebindingOperation = action.action.PerformInteractiveRebinding();
        }
        
        rebindingOperation
           // .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RefreshContent()
    {
        int bindingIndex;
        if (isComposite && name != String.Empty) {
            bindingIndex = action.action.bindings.IndexOf(x => x.isPartOfComposite && x.name == name);
        }
        else
        {
            bindingIndex = action.action.GetBindingIndexForControl(action.action.controls[0]);
        }
        
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
            action.action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();
        bindButton.gameObject.SetActive(true);
        RefreshContent();
    }
}
