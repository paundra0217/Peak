using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Interactable
{
    public string InteractableName;
    public UnityEvent InteractableEvent;
}

public class InteractableManager : MonoBehaviour
{
    public List<Interactable> interactables;

    private string InteractableAreaName;

    private static InteractableManager _instance;
    public static InteractableManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Interactable Manager is null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        print(_instance);
    }

    public void SetInteractableAreaName(string name)
    {
        InteractableAreaName = name;
    }

    public void ClearInteractableAreaName()
    {
        InteractableAreaName = null;
    }

    public void TriggerInteractable()
    {
        if (InteractableAreaName == null) return;
        
        Interactable ie = interactables.FirstOrDefault(i => i.InteractableName == InteractableAreaName);
        if (ie == null)
            Debug.LogError("Interaction is not valid");

        ie.InteractableEvent.Invoke();
    }

    public void AutoTriggerInteractable(string name)
    {
        Interactable ie = interactables.FirstOrDefault(i => i.InteractableName == name);
        if (ie == null)
            Debug.LogError("Interaction is not valid");

        ie.InteractableEvent.Invoke();
    }

    public void TriggerGameOver()
    {
        Interactable ie = interactables.FirstOrDefault(i => i.InteractableName == "GameOverScreen");
        if (ie == null)
            Debug.LogError("Interaction is not valid");

        ie.InteractableEvent.Invoke();
    }
}
