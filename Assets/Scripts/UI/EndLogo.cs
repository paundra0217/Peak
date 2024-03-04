using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLogo : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine("PlayEndLogo");
    }

    IEnumerator PlayEndLogo()
    {
        yield return new WaitForSeconds(5f);

        Transition.Instance.SwitchScene("MainMenu");
    }
}
