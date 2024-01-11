using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float HP;
    private float defaultHP;

    public void SetDefaultHP(float hp) { 
        defaultHP = hp;
    }

    public void Heal(float hp)
    {
        HP += hp;
    }

    public void TakeDamage(float hp)
    {
        HP -= hp;

        if (HP <= 0)
        {
            GameManager.Instance.PlayerDeath();
        }
    }

    public void ResetHealth()
    {
        HP = defaultHP;
    }

    public float GetHealth()
    {
        return HP;
    }
}
