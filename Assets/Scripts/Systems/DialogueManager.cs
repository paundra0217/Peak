using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject DialogueBox;
    private TMP_Text dialogueSpeaker;
    private TMP_Text dialogueContent;
    private GameObject dialogueArrow;
    private Animator dialogueAnimator;
    private Queue<Dialogue> content;
    private DialogueContents selectedDialogue;
    private string currentDialogueContent;
    private string nextDialoguePart;
    private bool IsAnimating;
    private bool IsTransitioning; //untuk pas lg animasi buka/tutup
    private GameStatus previousStatus;

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
        dialogueSpeaker = DialogueBox.transform.Find("TxtSpeaker").GetComponent<TMP_Text>();
        dialogueContent = DialogueBox.transform.Find("TxtContent").GetComponent<TMP_Text>();
        dialogueArrow = DialogueBox.transform.Find("ImgArrow").gameObject;
        dialogueAnimator = DialogueBox.GetComponent<Animator>();
    }

    public void StartDialogue(string DialoguePart)
    {
        if (GameManager.Instance.CompareStatus(GameStatus.DEFAULT) || GameManager.Instance.CompareStatus(GameStatus.INTRO))
            previousStatus = GameManager.Instance.GetStatus();

        GameManager.Instance.ChangeStatus(GameStatus.DIALOGUE);

        selectedDialogue = transform.Find(DialoguePart).GetComponent<DialogueContents>();
        if (selectedDialogue == null)
        {
            Debug.LogErrorFormat("Dialogue {0} not found on the database.", DialoguePart);
            return;
        }

        content.Clear();
        List<Dialogue> dialogueContents = selectedDialogue.dialogue;

        foreach (Dialogue dc in dialogueContents)
        {
            content.Enqueue(dc);
        }

        Debug.LogFormat("Starting Dialogue {0}", DialoguePart);

        DialogueBox.SetActive(true);

        if (selectedDialogue.isATip) DialogueBox.GetComponent<Image>().color = Color.yellow;
        else DialogueBox.GetComponent<Image>().color = Color.white;

        StartCoroutine("DialogueEnter");
    }

    public void ContinueDialogue()
    {
        StartDialogue(nextDialoguePart);
    }

    public void ChangeDialogue()
    {
        if (IsTransitioning) return;

        if (IsAnimating) SkipDialogueAnimation();
        else NextDialogue();
    }

    private void SkipDialogueAnimation()
    {
        StopCoroutine("DialogueAnimating");
        dialogueContent.text = currentDialogueContent;
        IsAnimating = false;
        dialogueArrow.SetActive(true);
    }

    private void NextDialogue()
    {
        IsAnimating = true;

        dialogueArrow.SetActive(false);

        if (content.Count == 0)
        {
            EndDialogue();
            return;
        }

        Dialogue contentToDisplay = content.Dequeue();
        dialogueSpeaker.text = contentToDisplay.characterName;
        dialogueContent.text = "";
        currentDialogueContent = contentToDisplay.dialogueContent;

        StartCoroutine("DialogueAnimating");
    }

    private void EndDialogue()
    {
        Debug.LogFormat("Conversation ended");

        StartCoroutine("DialogueLeave");
    }

    IEnumerator DialogueEnter()
    {
        IsTransitioning = true;

        yield return new WaitForSeconds(dialogueAnimator.GetCurrentAnimatorStateInfo(0).length);

        IsTransitioning = false;

        NextDialogue();
    }

    IEnumerator DialogueAnimating()
    {
        foreach (char c in currentDialogueContent.ToCharArray())
        {
            dialogueContent.text += c;
            yield return new WaitForSeconds(0.01f);
        }

        IsAnimating = false;
        dialogueArrow.SetActive(true);
    }

    IEnumerator DialogueLeave()
    {
        IsTransitioning = true;

        dialogueSpeaker.text = "";
        dialogueContent.text = "";

        dialogueAnimator.SetTrigger("TriggerAnimation");

        yield return new WaitForSeconds(dialogueAnimator.GetCurrentAnimatorStateInfo(0).length);

        DialogueBox.SetActive(false);

        IsTransitioning = false;

        if (selectedDialogue.afterDialogueEvent.GetPersistentEventCount() > 0)
        {
            if (selectedDialogue.nextDialoguePart != "")
                nextDialoguePart = selectedDialogue.nextDialoguePart;

            selectedDialogue.afterDialogueEvent.Invoke();
            yield break;
        }

        GameManager.Instance.ChangeStatus(previousStatus);
    }
}
