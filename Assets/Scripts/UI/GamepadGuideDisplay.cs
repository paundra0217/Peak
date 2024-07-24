using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamepadGuideDisplay : MonoBehaviour
{
    [SerializeField] private bool signInteractableMode;
    private CanvasGroup cg;
    private TMP_Text text;
    private bool isInGame;

    // Start is called before the first frame update
    void Awake()
    {
        if (signInteractableMode)
        {
            isInGame = true;
            cg = transform.Find("GamepadGuide").GetComponent<CanvasGroup>();
            text = transform.Find("TxtInteract").GetComponent<TMP_Text>();
        }
        else
        {
            cg = GetComponent<CanvasGroup>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInGame)
        {
            bool isUsingController = GameManager.Instance.GetIsUsingController();
            cg.alpha = isUsingController ? 1f : 0f;
            text.alpha = isUsingController ? 0f : 1f;
        }
        else
        {
            cg.alpha = GameManager.Instance.GetIsUsingController() ? 1f : 0f;
        }
    }
}
