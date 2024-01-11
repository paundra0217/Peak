using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableArea : MonoBehaviour
{
    [SerializeField] private string InteractableAreaName;
    [SerializeField] private bool IsAutomatic;

    private void Awake()
    {
        if (InteractableAreaName == null)
            Debug.LogError("Interactable Area Name is not set.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (InteractableAreaName == null) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        if (IsAutomatic)
            InteractableManager.Instance.AutoTriggerInteractable(InteractableAreaName);
        else
            InteractableManager.Instance.SetInteractableAreaName(InteractableAreaName);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsAutomatic || InteractableAreaName == null) return;

        if (collision.gameObject.CompareTag("Player"))
            InteractableManager.Instance.ClearInteractableAreaName();
    }
}
