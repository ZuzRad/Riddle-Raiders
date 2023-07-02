using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Action onPlayersDeath;
    public Action onPlayerHit;
    
    [Header("Health value")]
    private int health = 3;
    [SerializeField] private GameObject skullEffect;
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject smokeEffect;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Spikes spikes = col.GetComponent<Spikes>();
        Lava lava = col.GetComponent<Lava>();

        if (spikes)
        {
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
            DecreaseHealth();
        }
        if (lava)
        {
            while(health > 0)
            {
                DecreaseHealth();
            }
            Instantiate(smokeEffect, transform.position, Quaternion.identity);
        }
    }

    private void DecreaseHealth()
    {
        if (--health == 0)
        {
            Destroy(gameObject);
            onPlayersDeath?.Invoke();
            Instantiate(skullEffect, transform.position, Quaternion.identity);
        }
        gameObject.GetComponent<Animator>().SetTrigger("attack");
        onPlayerHit?.Invoke();
        
    }
}
