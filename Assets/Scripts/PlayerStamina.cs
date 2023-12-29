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

    // Start is called before the first frame update
    void Awake()
    {
        defaultSTP = STP;    
    }

    public void SetStamina(float stp)
    {
        STP = stp;
    }

    public void ResetStamina()
    {
        STP = defaultSTP;
    }

    public float GetStamina()
    {
        return STP;
    }

    public void DepleteStamina()
    {

    }
}
