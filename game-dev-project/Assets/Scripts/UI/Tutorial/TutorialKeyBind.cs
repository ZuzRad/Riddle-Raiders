using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialKeyBind : MonoBehaviour
{
    [SerializeField] private InputActionReference action;
    [SerializeField] private bool isComposite;
    [SerializeField] private string name;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI bindingDisplayNameText;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Start()
    {
        RefreshContent();
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
}
