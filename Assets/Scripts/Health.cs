using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int HP = 100;

    private int defaultHP;

    // Start is called before the first frame update
    void Awake()
    {
        defaultHP = HP;
    }

    public void Heal(int hp)
    {
        HP += hp;
    }

    public void TakeDamage(int hp)
    {
        HP -= hp;

        if (HP < 0)
        {
            // death action
        }
    }

    public void ResetHealth()
    {
        HP = defaultHP;
    }

    public int GetHealth()
    {
        return HP;
    }
}
