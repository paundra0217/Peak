using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstantTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent triggerEvent;
    [SerializeField] private bool triggerOnce = true;
    private bool alreadyTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (triggerOnce && alreadyTriggered) return;

        if (triggerEvent.GetPersistentEventCount() < 0)
        {
            Debug.LogWarning("No events we're assigned");
            return;
        }

        triggerEvent.Invoke();
    }
}
