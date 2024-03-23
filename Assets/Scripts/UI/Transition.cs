using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TransitionSet
{
    public string setName;
    public string message;
    public float duration;
    public bool onlyTriggeredOnce;
    public bool isAlreadyTriggered;
    public bool continueDialogueAfterTransition;
    public bool PauseHalfway;
    public UnityEvent EventDuringTransition;
    public UnityEvent EventAfterTransition;
    public UnityEvent EventAfterTransitionAnimation;
}

public class Transition : MonoBehaviour
{
    [SerializeField] private List<TransitionSet> transitionSets;

    private TransitionSet selectedSet;
    private TMP_Text transitionText;
    private Animator animator;
    private string nextScene;
    private bool isBlack;
    private bool PausedHalfway;
    
    private static Transition _instance;
    public static Transition Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Transition Manager is Null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        print(_instance);
        transitionText = transform.Find("TransitionText").GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    public void DoTransition(string transitionSetName)
    {
        if (PausedHalfway) return;

        selectedSet = transitionSets.FirstOrDefault(t => t.setName == transitionSetName);
        if (selectedSet == null)
        {
            Debug.LogErrorFormat("{0} is not found.", transitionSetName);
            return;
        }

        if (selectedSet.isAlreadyTriggered)
        {
            Debug.LogWarningFormat("{0} cannot be triggered because already been triggered and has been set to triggered once", selectedSet.setName);
            return;
        }

        if (selectedSet.onlyTriggeredOnce)
            selectedSet.isAlreadyTriggered = true;

        GameManager.Instance.ChangeStatus(GameStatus.TRANSITION);
        if (selectedSet.PauseHalfway)
        {
            gameObject.SetActive(true);
            isBlack = true;

            StartCoroutine(ProcessSingleFade());
        }
        else
        {
            StartCoroutine("ProcessTransitionOpen");
        }
    }

    public void DoGameOver()
    {
        StartCoroutine("ProcessGameOverScreen");
    }

    IEnumerator ProcessTransitionOpen()
    {
        animator.SetTrigger("TriggerTransition");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        transitionText.text = "";
        if (selectedSet.EventDuringTransition.GetPersistentEventCount() > 0)
            selectedSet.EventDuringTransition.Invoke();

        if (selectedSet.message == "")
        {
            StartCoroutine("ProcessTransitionClose");
        }
        else
        {
            StartCoroutine("AnimateText");
        }
    }

    IEnumerator AnimateText()
    {
        foreach (char c in selectedSet.message.ToCharArray())
        {
            transitionText.text += c;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine("ProcessTransitionClose");
    }

    IEnumerator ProcessTransitionClose()
    {
        yield return new WaitForSeconds(selectedSet.duration);

        transitionText.text = "";

        if (selectedSet.EventAfterTransition.GetPersistentEventCount() > 0)
            selectedSet.EventAfterTransition.Invoke();

        animator.SetTrigger("TriggerTransition");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (selectedSet.continueDialogueAfterTransition)
            DialogueManager.Instance.ContinueDialogue();
        else if (selectedSet.EventAfterTransitionAnimation.GetPersistentEventCount() > 0)
            selectedSet.EventAfterTransitionAnimation.Invoke();
        else
            GameManager.Instance.ChangeStatus(GameStatus.DEFAULT);
    }

    public void ContinueTransition()
    {
        if (!PausedHalfway) return;

        StartCoroutine("ProcessSingleFade");
    }

    IEnumerator ProcessSingleFade()
    {
        if (PausedHalfway)
        {
            if (selectedSet.EventAfterTransition.GetPersistentEventCount() > 0)
                selectedSet.EventAfterTransition.Invoke();
        }

        animator.SetTrigger("TriggerTransition");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        PausedHalfway = !PausedHalfway;

        if (PausedHalfway)
        {
            if (selectedSet.EventDuringTransition.GetPersistentEventCount() > 0)
                selectedSet.EventDuringTransition.Invoke();
        }
        else
        {
            if (selectedSet.continueDialogueAfterTransition)
                DialogueManager.Instance.ContinueDialogue();
            else if (selectedSet.EventAfterTransitionAnimation.GetPersistentEventCount() > 0)
                selectedSet.EventAfterTransitionAnimation.Invoke();
            else
                GameManager.Instance.ChangeStatus(GameStatus.DEFAULT);
        }

        if (!isBlack)
        {
            gameObject.SetActive(false);
        }
        else
        {
            isBlack = false;
        }
    }

    public void SwitchScene(string nextScene)
    {
        this.nextScene = nextScene;
        StartCoroutine("ProcessSwitchScene");
    }

    IEnumerator ProcessSwitchScene()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<CanvasGroup>().interactable = true;

        animator.SetTrigger("TriggerTransition");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene(nextScene);
    }

    IEnumerator ProcessGameOverScreen()
    {
        animator.SetTrigger("TriggerTransition");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        SceneManager.LoadScene("GameOver");
    }
}
