using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LaserReciever : PlatformActivator
{
    public Action onLaserActivation;
    public bool isActive = false;
    private bool wasActivationEventSent = false;
    private bool wasInactivationEventSent = false;
    [SerializeField] private GameObject laserEffect;

    void Update() {
        if (isActive)
        {
            if (!wasActivationEventSent) 
            {
                Instantiate(laserEffect, transform.position, Quaternion.identity);
                onLaserActivation?.Invoke();
                wasActivationEventSent = true;
                wasInactivationEventSent = false;
                onChangeState?.Invoke(State.On, platformColor);
            }
        }
        else 
        {
            if (!wasInactivationEventSent)
            {
                wasInactivationEventSent = true;
                wasActivationEventSent = false;
                onChangeState?.Invoke(State.Off, platformColor);
            }
        }
        
        isActive = false;
    }
}
