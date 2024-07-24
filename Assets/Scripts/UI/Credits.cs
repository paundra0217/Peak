using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private float skipConfirmCooldown = 3f;
    [SerializeField] private TMP_Text confirmText;
    private Animator animator;
    private float cooldownTime;
    private bool confirmSkipCredits;

    private static Credits _instance;
    public static Credits Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Credits is Null");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.anyKeyDown && GameManager.Instance.CompareStatus(GameStatus.ENDING))
            ConfirmSkipCredits();

        if (confirmSkipCredits && cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
            confirmText.alpha = 1f;
        }
        else
        {
            confirmSkipCredits = false;
            confirmText.alpha = 0f;
        }
    }

    public void StartCredits()
    {
        GameManager.Instance.TriggerCredits();
        gameObject.SetActive(true);
        StartCoroutine("HandleCredits");
    }

    private void ConfirmSkipCredits()
    {
        print("hello");
        if (!confirmSkipCredits)
        {
            confirmSkipCredits = true;
            cooldownTime = skipConfirmCooldown;
        }
        else
            SkipCredits();
    }

    private void SkipCredits()
    {
        Transition.Instance.SwitchScene("PlayResult");
    }

    IEnumerator HandleCredits()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Transition.Instance.SwitchScene("EndScreen");
    }
}
