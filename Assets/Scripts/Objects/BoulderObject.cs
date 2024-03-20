using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderObject : MonoBehaviour
{
    private Rigidbody2D rb2;
    private bool alreadyTriggered;

    // Start is called before the first frame update
    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        //rb2.simulated = true;
    }

    public void TriggerBoulder()
    {
        if (!alreadyTriggered) alreadyTriggered = true;
        else return;

        rb2.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("AutoKill"))
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.KillPlayer();
        }
    }
}
