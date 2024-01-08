using System.Collections;
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

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Dialogue")]
public class DialogueContents : ScriptableObject
{
    public List<Dialogue> dialogue;
    public UnityEvent afterDialogueEvent;
    public DialogueContents nextDialogueContent;
}
