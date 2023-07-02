using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Button : PlatformActivator
{
    [Header("Appearance")] 
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite pressedButton;
    [SerializeField] private Sprite releasedButton;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character player = collision.GetComponent<Character>();
        if (player == null) return;
        
        if (player.Type == Character.CharacterType.Big)
        {
            renderer.sprite = pressedButton;
            onChangeState?.Invoke(State.On, platformColor);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Character player = collision.GetComponent<Character>();
        if (player == null) return;
        
        if (player.Type == Character.CharacterType.Big)
        {
            renderer.sprite = releasedButton;
            onChangeState?.Invoke(State.Off, platformColor);
        }
    }
}
