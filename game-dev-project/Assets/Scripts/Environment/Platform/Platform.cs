using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformColor Color => platformColor;
    [SerializeField] private Transform shiftTransform;
    [SerializeField] private PlatformColor platformColor;
    [SerializeField] private float shiftingSpeed;

    private bool inShiftMode;
    private Vector3 originalPosition;
    private Vector3 shiftPosition;
    private Vector3 currentDestitation;
    
    private void Start() {
        originalPosition = transform.position;
        shiftPosition = shiftTransform.position;
    }
    
    private void Update()
    {
        ChangePosition();
    }

    public void StartShifting()
    {
        inShiftMode = true;
        currentDestitation = shiftPosition;
    }

    public void StopShifting()
    {
        inShiftMode = true;
        currentDestitation = originalPosition;
    }
    
    private void ChangePosition()
    {
        if (inShiftMode)
        {
            transform.position = Vector3.Lerp(transform.position, currentDestitation, shiftingSpeed);
            
            if (Vector3.Distance(transform.position, currentDestitation) <= 0.001f) 
            {
                inShiftMode = false;
                transform.position = currentDestitation;
            }
        }
    }
    
    public enum PlatformColor {
        Pink,
        Green,
        Blue,
        White,
        Yellow,
        Orange
    }
}
