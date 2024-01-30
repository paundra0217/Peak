using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class TransitionSet
{
    public string setName;
    public string message;
    public float duration;
    public bool onlyTriggeredOnce;
    public bool isAlreadyTriggered;
    public bool continueDialogueAfterTransition;
    public UnityEvent EventAfterTransition;
    public UnityEvent EventAfterTransitionAnimation;
}

public class Transition : MonoBehaviour
{
    [SerializeField] private List<TransitionSet> transitionSets;

    private TransitionSet selectedSet;
    private TMP_Text transitionText;
    private Animator animator;
    
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
        transitionText = transform.Find("TransitionText").GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
    }

    public void DoTransition(string transitionSetName)
    {
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

        gameObject.SetActive(true);
        GameManager.Instance.ChangeStatus(GameStatus.TRANSITION);
        StartCoroutine("ProcessTransition");
    }

    IEnumerator ProcessTransition()
    {
        animator.SetBool("IsTransitioning", true);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        transitionText.text = selectedSet.message;

        yield return new WaitForSeconds(selectedSet.duration);

        transitionText.text = "";

        if (selectedSet.EventAfterTransition.GetPersistentEventCount() > 0)
            selectedSet.EventAfterTransition.Invoke();

        animator.SetBool("IsTransitioning", false);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (selectedSet.continueDialogueAfterTransition)
            DialogueManager.Instance.ContinueDialogue();
        else if (selectedSet.EventAfterTransitionAnimation.GetPersistentEventCount() > 0)
            selectedSet.EventAfterTransitionAnimation.Invoke();
        else
            GameManager.Instance.ChangeStatus(GameStatus.DEFAULT);

        gameObject.SetActive(false);
    }
}
