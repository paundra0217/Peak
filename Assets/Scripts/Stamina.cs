using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float STP = 100f;

    private float defaultSTP;

    // Start is called before the first frame update
    void Awake()
    {
        defaultSTP = STP;    
    }

    public void SetStamina(int stp)
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
}
