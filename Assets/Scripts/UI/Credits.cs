using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private Animator animator;

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
        print(_instance);
        animator = GetComponent<Animator>();
    }

    public void StartCredits()
    {
        GameManager.Instance.TriggerCredits();
        gameObject.SetActive(true);
        StartCoroutine("HandleCredits");
    }

    IEnumerator HandleCredits()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Transition.Instance.SwitchScene("EndScreen");
    }
}
