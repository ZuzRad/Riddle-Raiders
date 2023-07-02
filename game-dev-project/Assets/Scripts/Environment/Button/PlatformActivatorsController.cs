using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivatorsController : MonoBehaviour {
    public Action<PlatformActivator.State, Platform.PlatformColor> onStateChange;
    private PlatformActivator[] platformActivators;

    private void Start() {
        GetActivatorsReference();
    }

    private void GetActivatorsReference()
    {
        platformActivators = GetComponentsInChildren<PlatformActivator>(true);
        
        foreach (var activator in platformActivators)
        {
            activator.onChangeState += (state, color) =>
            {
                onStateChange?.Invoke(state, color);
            };
        }
    }
}
