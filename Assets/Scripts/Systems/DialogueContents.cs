using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string characterName;
    
    [TextArea(3, 10)]
    public string dialogueContent;
}

public class DialogueContents : MonoBehaviour
{
    public List<Dialogue> dialogue;
    public UnityEvent afterDialogueEvent;
    public string nextDialoguePart;
    public bool isATip;
}
