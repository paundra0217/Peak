using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(InteractableArea), true), CanEditMultipleObjects]
public class InteractableAreaEditor : Editor
{
    // this are serialized variables in YourClass
    SerializedProperty IsAutomatic;
    SerializedProperty runOnlyOnce;
    SerializedProperty customSpawnLocation;


    private void OnEnable()
    {
        IsAutomatic = serializedObject.FindProperty("IsAutomatic");
        runOnlyOnce = serializedObject.FindProperty("runOnlyOnce");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(IsAutomatic);

        if (IsAutomatic.boolValue)
        {
            EditorGUILayout.PropertyField(runOnlyOnce);
        }

        // must be on the end.
        serializedObject.ApplyModifiedProperties();

        // add this to render base
        base.OnInspectorGUI();

    }
}

public class InteractableArea : MonoBehaviour
{
    [SerializeField] private string InteractableAreaName;
    [SerializeField] private bool IsAutomatic;
    [SerializeField] private bool runOnlyOnce;

    private CanvasGroup cg;
    private bool alreadyTriggered = false;

    private void Awake()
    {
        if (transform.Find("Canvas") != null)
            cg = transform.Find("Canvas").GetComponent<CanvasGroup>();
        
        if (InteractableAreaName == null)
            Debug.LogError("Interactable Area Name is not set.");

        if (cg != null)
            cg.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (runOnlyOnce && alreadyTriggered) return;

        if (InteractableAreaName == null) return;

        if (!collision.gameObject.CompareTag("Player")) return;

        if (runOnlyOnce) alreadyTriggered = true;

        if (InteractableManager.Instance.CheckInteractStatus(InteractableAreaName)) return;

        if (IsAutomatic)
            InteractableManager.Instance.AutoTriggerInteractable(InteractableAreaName);
        else
        {
            cg.alpha = 1f;
            InteractableManager.Instance.SetInteractableAreaName(InteractableAreaName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsAutomatic || InteractableAreaName == null) return;

        if (cg != null)
            cg.alpha = 0f;

        if (collision.gameObject.CompareTag("Player"))
            InteractableManager.Instance.ClearInteractableAreaName();
    }

    private void Update()
    {
        if (GameManager.Instance.CompareStatus(GameStatus.DEFAULT) || GameManager.Instance.CompareStatus(GameStatus.INTRO)) return;
        
        if (cg != null)
            cg.alpha = 0f;
        //cg.alpha = 0f;
        ////print(cg.transform.Find("TxtInteract").GetComponent<TMP_Text>().text);
        ////print(gameObject);
        //if (cg != null)
        //{
        //    print(gameObject.name);
        //    print(cg.transform.Find("TxtInteract").GetComponent<TMP_Text>().text);
        //}
    }
}
