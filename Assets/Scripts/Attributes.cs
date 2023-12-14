using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    [SerializeField] private int Health = 100;
    [SerializeField] private int Speed = 20;
    [SerializeField] private int Stamina = 10;

    private int defaultHealth;
    private int defaultSpeed;
    private int defaultStamina;

    private void Awake()
    {
        defaultHealth = Health;
        defaultSpeed = Speed;
        defaultStamina = Stamina;
    }

    public void Heal(int hp)
    {
        Health += hp;
    }

    public void TakeDamage(int hp)
    {
        Health -= hp;

        if (Health < 0)
        {
            // death action
        }
    }

    public void ResetHealth()
    {
        Health = defaultHealth;
    }

    public int GetHealth()
    {
        return Health;
    }

    public void SetSpeed(int speed)
    {
        Speed = speed;
    }

    public void ResetSpeed()
    {
        Speed = defaultSpeed;
    }

    public int GetSpeed()
    {
        return Speed;
    }

    public void SetStamina(int stamina)
    {
        Stamina = stamina;
    }

    public void ResetStamina()
    {
        Stamina = defaultStamina;
    }

    public int GetStamina()
    {
        return Stamina;
    }
}
