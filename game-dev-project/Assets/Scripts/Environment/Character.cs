using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public Action onPlayersDeath;
    public Action<Character> onPlayerHit;
    public Movement Movement => movement;
    [field:SerializeField] public CharacterType Type { get; private set; }
    [SerializeField] Movement movement;
    [SerializeField] HealthManager healthManager;
    [SerializeField] SoundManager soundManager;
    
    private void Start() 
    {
        healthManager.onPlayersDeath += () =>
        {
            onPlayersDeath?.Invoke();
        };
        
        healthManager.onPlayerHit += () =>
        {
            soundManager.PlaySound(SoundManager.Sounds.Hit);
            onPlayerHit?.Invoke(this);
        };

        movement.onCatch += () => soundManager.PlaySound(SoundManager.Sounds.Box);
        movement.onJump += () => soundManager.PlaySound(SoundManager.Sounds.Jump);
        movement.onSlide += () => soundManager.PlaySound(SoundManager.Sounds.Slide);
        
        movement.onStartClimb += () =>
        {
            soundManager.SetLoop(true);
            soundManager.SetClip(SoundManager.Sounds.Climb);
        };
        
        movement.onStopClimb += () =>
        {
            soundManager.SetLoop(false);
            soundManager.StopPlaying();
        };
    }

    public enum CharacterType 
    {
        Big,
        Small
    }
}
