using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Boundaries
{
    public string BoundariesName;
    public GameObject BoundariesObject;
}

public class CameraBoundsController : MonoBehaviour
{
    [SerializeField] private Boundaries[] bounds;
    private GameObject activeBound;

    private static CameraBoundsController _instance;
    public static CameraBoundsController Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Camera Bounds Controller is Null");
            }

            return _instance;
        }

    }
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

    public GameObject SwitchBoundaries(string name)
    {
        foreach (var b in bounds)
        {
            if (b.BoundariesName == name)
            {
                activeBound = b.BoundariesObject;
                b.BoundariesObject.SetActive(true);
            }
            else
                b.BoundariesObject.SetActive(false);
        }

        return activeBound;
    }
}
