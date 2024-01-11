using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class HarmfulObject : MonoBehaviour
{
    [SerializeField] private float Damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnContact(collision);
    }

    public abstract void OnContact(Collision2D collision);
}
