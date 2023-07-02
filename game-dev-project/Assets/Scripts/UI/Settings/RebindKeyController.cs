using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindKeyController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    
    private const string player1KeyBindingsConfKey = "PLAYER_1_BINDINGS";
    private const string player2KeyBindingsConfKey = "PLAYER_2_BINDINGS";
    private const string player1ActionMap = "Player1";
    private const string player2ActionMap = "Player2";

    private void Awake()
    {
        LoadKeyBindings();
    }

    public void SaveKeyBindings()
    {
        SaveKeyBindings(player1KeyBindingsConfKey, player1ActionMap);
        SaveKeyBindings(player2KeyBindingsConfKey, player2ActionMap);
    }
    
    private void LoadKeyBindings()
    {
        LoadKeyBindings(player1KeyBindingsConfKey, player1ActionMap);
        LoadKeyBindings(player2KeyBindingsConfKey, player2ActionMap);
    }
    
    private void SaveKeyBindings(string confKey, string actionMap)
    {
        string rebinds = playerInput.actions.FindActionMap(actionMap).SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(confKey, rebinds);
    }

    private void LoadKeyBindings(string confKey, string actionMap)
    {
        string rebinds = PlayerPrefs.GetString(confKey, string.Empty);

        if (!string.IsNullOrEmpty(rebinds))
        {
            playerInput.actions.FindActionMap(actionMap).LoadBindingOverridesFromJson(rebinds);
        }
    }
}
