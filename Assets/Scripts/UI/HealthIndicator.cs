using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    private CanvasGroup cg;
    private TMP_Text txtHealth;
    private Image imgHealth;

    // Start is called before the first frame update
    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        txtHealth = transform.Find("TxtHealth").GetComponent<TMP_Text>();
        imgHealth = transform.Find("HealthIconFill").GetComponent<Image>();
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
            imgHealth.fillAmount = GameManager.Instance.GetHealth() / GameManager.Instance.GetMaxHealth();
            txtHealth.text = GameManager.Instance.GetLives().ToString() + "x";
        }
    }
}
