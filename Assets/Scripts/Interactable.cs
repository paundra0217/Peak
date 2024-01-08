using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private bool EnterArea;
    public UnityEvent TriggerInteract;

    private void Awake()
    {
        if (TriggerInteract == null)
            TriggerInteract = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnterArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EnterArea = false;
        }
    }

    private void Update()
    {
        if (!EnterArea) return;

        if (Input.GetKeyDown(KeyCode.E))
            TriggerInteract.Invoke();
    }
}
