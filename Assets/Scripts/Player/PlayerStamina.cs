using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAction
{
    
}

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] private float STP = 100f;

    private float defaultSTP;
    private float depletionRate = 1f;
    private float cooldownDamage = 1f;
    private int damageCount = 0;
    private float currentCooldown;

    // Start is called before the first frame update
    void Awake()
    {
        defaultSTP = STP;
        depletionRate = 0f;
        currentCooldown = cooldownDamage;
        //STP = 50f;
    }

    private void Update()
    {
        if (STP > 0f) return;
        if (damageCount > 30) return;
        if (GameManager.Instance.CompareStatus(GameStatus.DEATH)) return;

        print(currentCooldown);
        
        if (damageCount == 30)
        {
            GameManager.Instance.KillPlayer();
            damageCount++;
        }
        else if (currentCooldown <= 0f)
        {
            GameManager.Instance.TakeDamage(1);
            damageCount++;
            currentCooldown = cooldownDamage;
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void SetStamina(float stp)
    {
        STP = stp;
    }

    public void SetDefaultStamina(float stp)
    {
        defaultSTP = stp;
    }

    public void ResetStamina()
    {
        STP = defaultSTP;
    }

    public float GetStamina()
    {
        return STP;
    }

    public float GetMaxStamina()
    {
        return defaultSTP;
    }

    public void SetDepletionRate(float depletionRate)
    {
        this.depletionRate = depletionRate;
    }

    public void DepleteStamina()
    {
<<<<<<< Updated upstream
        STP = STP * (1 - (0.0005f * 1));
=======
        STP = STP * (1 - (0.00015f * depletionRate));
>>>>>>> Stashed changes
        //STP = 0f;
        if (STP < 0f)
        {
            STP = 0f;
        }
    }
}
