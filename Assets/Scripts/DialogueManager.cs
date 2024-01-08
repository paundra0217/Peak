using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class DialogueSO
{
    public string dialoguePart;
    public DialogueContents dialogueSO;
}

public class DialogueManager : MonoBehaviour
{
    public List<DialogueSO> dialogues;
    private Queue<Dialogue> content;
    private DialogueSO selectedDialogue;
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

        selectedDialogue = dialogues.FirstOrDefault(d => d.dialoguePart == DialoguePart);
        if (selectedDialogue == null)
        {
            Debug.LogErrorFormat("Dialogue {0} not found on the database.", DialoguePart);
            return;
        }

        dialogues.Clear();
        List<Dialogue> dialogueContents = selectedDialogue.dialogueSO.dialogue;

        foreach(Dialogue dc in dialogueContents)
        {
            content.Enqueue(dc);
        }

        Debug.LogFormat("Starting Dialogue {0}", DialoguePart);

        NextDialogue();
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

        if (selectedDialogue.dialogueSO.afterDialogueEvent != null)
        {
            GameManager.Instance.ChangeStatus(GameStatus.TRANSITION);
            selectedDialogue.dialogueSO.afterDialogueEvent.Invoke();
            return;
        }

        GameManager.Instance.ChangeStatus(GameStatus.DEFAULT);
    }
}
