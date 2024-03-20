using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private CanvasGroup cg;
    private Image imgStamina;

    // Start is called before the first frame update
    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        imgStamina = transform.Find("StaminaFill").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GetGameplayStatus())
        {
            cg.alpha = 0f;
        }
        else
        {
            cg.alpha = 1f;
            gameObject.SetActive(true);
            imgStamina.fillAmount = GameManager.Instance.GetStamina() / GameManager.Instance.GetMaxStamina();
        }
    }
}
