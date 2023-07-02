using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : MonoBehaviour 
{
    public Action<State, Platform.PlatformColor> onChangeState;
    protected State state = State.None;
    [SerializeField] protected Platform.PlatformColor platformColor;

    protected void ChangeState(State state) {
        this.state = state;
        onChangeState?.Invoke(state, platformColor);
    }

    public enum State
    {
        None,
        Off,
        On
    }
}
