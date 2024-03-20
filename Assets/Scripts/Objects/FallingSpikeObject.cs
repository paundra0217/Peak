using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeObject : MonoBehaviour
{
    private Rigidbody2D rb2;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    public void TriggerFallingSpike()
    {
        rb2.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision detected");
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.TakeDamage(25);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
