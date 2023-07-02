using System;
using UnityEngine;

public class Geyser : MonoBehaviour
{
    public Action onGeyserActivation;
    
    [Header("Jump settings")]
    [SerializeField] private float power;

    
    private void AddForce(Collider2D collision)
    {
        if (collision.name == "Small Player" || collision.name == "Big Player")
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2(0, power-rb.velocity.y);
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onGeyserActivation?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AddForce(collision);
    }
}
