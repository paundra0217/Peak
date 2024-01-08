using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;
    public static TransitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Transition Manager is Null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
