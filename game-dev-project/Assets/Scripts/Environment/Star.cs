using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour 
{
    public Action onStartCollect;
    [SerializeField] private GameObject pickupStartEffect;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Character character = col.GetComponent<Character>();

        if (character)
        {
            Instantiate(pickupStartEffect, transform.position, Quaternion.identity);
            onStartCollect?.Invoke();
            Destroy(gameObject);
        }
    }
}
