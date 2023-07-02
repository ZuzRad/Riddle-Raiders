using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Action<float> onTimeTick;
    public bool IsCounting { get; set; }
    public float CurrentTime => currentTime;
    private float currentTime = 0;
    
    private void Start() {
        IsCounting = true;
    }

    public void ResetTimer()
    {
        currentTime = 0;
    }
    
    private void Update()
    {
        if (IsCounting)
        {
            currentTime += Time.deltaTime;
            onTimeTick?.Invoke(currentTime);
        }
    }
}
