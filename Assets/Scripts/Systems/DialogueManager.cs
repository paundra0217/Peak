using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<Dialogue> content;
    private DialogueContents selectedDialogue;
    private string nextDialoguePart;
    private bool IsAnimating;

    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Dialogue Manager is Null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        content = new Queue<Dialogue>();
    }

    public void StartDialogue(string DialoguePart)
    {
        GameManager.Instance.ChangeStatus(GameStatus.DIALOGUE);

        selectedDialogue = transform.Find(DialoguePart).GetComponent<DialogueContents>();
        if (selectedDialogue == null)
        {
            Debug.LogErrorFormat("Dialogue {0} not found on the database.", DialoguePart);
            return;
        }

        content.Clear();
        List<Dialogue> dialogueContents = selectedDialogue.dialogue;

        foreach(Dialogue dc in dialogueContents)
        {
            content.Enqueue(dc);
        }

        Debug.LogFormat("Starting Dialogue {0}", DialoguePart);

        NextDialogue();
    }

    public void ContinueDialogue()
    {
        StartDialogue(nextDialoguePart);
    }

    public void ChangeDialogue()
    {
        NextDialogue();
    }

    private void SkipDialogueAnimation()
    {

    }

    private void NextDialogue()
    {
        if (content.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue contentToDisplay = content.Dequeue();
        Debug.LogFormat("{0}: {1}", contentToDisplay.characterName, contentToDisplay.dialogueContent);
    }

    private void EndDialogue()
    {
        Debug.LogFormat("Conversation ended");

        if (selectedDialogue.afterDialogueEvent.GetPersistentEventCount() > 0)
        {
            if (selectedDialogue.nextDialoguePart != "")
                nextDialoguePart = selectedDialogue.nextDialoguePart;

            selectedDialogue.afterDialogueEvent.Invoke();
            return;
        }

        GameManager.Instance.ChangeStatus(GameStatus.DEFAULT);
    }
}
