using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Movement movement;
    private Jump jump;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<Movement>();
        jump = GetComponent<Jump>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.CompareStatus(GameStatus.DIALOGUE))
        {
            if (Input.anyKeyDown)
                DialogueManager.Instance.ChangeDialogue();
        } 
        else if (GameManager.Instance.CompareStatus(GameStatus.DEFAULT) || GameManager.Instance.CompareStatus(GameStatus.INTRO))
        {
            movement.SetDirection(Input.GetAxisRaw("Horizontal"));

            if (!GameManager.Instance.CompareStatus(GameStatus.INTRO))
            {
                if (Input.GetKey(KeyCode.Space))
                    jump.DoJump();

                if (Input.GetKeyDown(KeyCode.Q))
                    jump.DoSoar();

                if (Input.GetKeyDown(KeyCode.E))
                    InteractableManager.Instance.TriggerInteractable();

                if (Input.GetKeyDown(KeyCode.R))
                    GameManager.Instance.TestFunction();
            }
        }
    }
}
