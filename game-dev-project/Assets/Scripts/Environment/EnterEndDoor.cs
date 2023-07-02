using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterEndDoor : MonoBehaviour
{
    public Action onEnterEndDoor;

    [SerializeField] GameObject openDoorSprite;
    [SerializeField] GameObject closeDoorSprite;
    private bool smallPlayerAtDoor = false;
    private bool bigPlayerAtDoor = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Character character = col.GetComponent<Character>();
        if (character == null) 
        {
            return;
        }
        
        if (character.Type == Character.CharacterType.Small)
        {
            smallPlayerAtDoor = true;
        }
        else if (character.Type == Character.CharacterType.Big)
        {
            bigPlayerAtDoor = true;
        }

        if (smallPlayerAtDoor && bigPlayerAtDoor)
        {
            openDoorSprite.SetActive(true);
            closeDoorSprite.SetActive(false);
            onEnterEndDoor?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Character character = col.GetComponent<Character>();
        
        if (character == null) 
        {
            return;
        }
        
        if (character.Type == Character.CharacterType.Small)
        {
            smallPlayerAtDoor = false;
        }
        else if (character.Type == Character.CharacterType.Big)
        {
            bigPlayerAtDoor = false;
        }
    }
}
