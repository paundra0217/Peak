using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class BackgroundObject
{
    public string backgroundName;
    public GameObject backgroundObject;
}

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private BackgroundObject[] backgrounds;

    private static BackgroundController _instance;
    public static BackgroundController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Background Controller is null");
            }

            return _instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    public void SwitchBackground(string name)
    {
        foreach (var bg in backgrounds)
        {
            if (bg.backgroundName == name)
                bg.backgroundObject.SetActive(true);
            else
                bg.backgroundObject.SetActive(false);
        }
    }
}
