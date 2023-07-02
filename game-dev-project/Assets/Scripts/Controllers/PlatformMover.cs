using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private PlatformController platformController;
    [SerializeField] private PlatformActivatorsController platformActivatorsController;

    private void Start()
    {
        platformActivatorsController.onStateChange += HandleActivatorStateChange;
    }

    private void OnDestroy()
    {
        platformActivatorsController.onStateChange -= HandleActivatorStateChange;
    }

    private void HandleActivatorStateChange(PlatformActivator.State state, Platform.PlatformColor color)
    {
        platformController.MovePlatforms(state, color);
    }
}
