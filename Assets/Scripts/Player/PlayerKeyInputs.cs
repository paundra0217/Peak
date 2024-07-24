using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKeyInputs : MonoBehaviour
{
    [SerializeField] private bool listeningForInputs;
    private Movement movement;
    private Jump jump;

    private float direction; 

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<Movement>();
        jump = GetComponent<Jump>(); 
    }

    //public void PlayerMoveControl(InputAction.CallbackContext context)
    //{
    //    direction = context.ReadValue<Vector2>().x;
    //}

    //public void PlayerJumpControl(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //        jump.DoJump();
    //}

    //public void PlayerSoarControl(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //        jump.DoSoar();
    //}

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetIsUsingController())
            direction = Input.GetAxisRaw("HorizontalController");
        else
            direction = Input.GetAxisRaw("Horizontal");

        if (GameManager.Instance.CompareStatus(GameStatus.DIALOGUE))
        {
            if (Input.anyKeyDown)
                DialogueManager.Instance.ChangeDialogue();

            return;
        }


        if (GameManager.Instance.CompareStatus(GameStatus.DEFAULT) || GameManager.Instance.CompareStatus(GameStatus.INTRO))
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2))
                InteractableManager.Instance.TriggerInteractable();

            if (!GameManager.Instance.CompareStatus(GameStatus.INTRO))
            {
                bool jumpKey = Application.platform == RuntimePlatform.OSXPlayer ? Input.GetKey(KeyCode.JoystickButton16) : Input.GetKey(KeyCode.JoystickButton0);
                if (Input.GetKey(KeyCode.Space) || jumpKey)
                    jump.DoJump();

                bool soarKey = Application.platform == RuntimePlatform.OSXPlayer ? Input.GetKeyDown(KeyCode.JoystickButton19) : Input.GetKeyDown(KeyCode.JoystickButton3);
                if (Input.GetKeyDown(KeyCode.Q) || soarKey)
                    jump.DoSoar();

                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                    GameManager.Instance.TestFunction();

                bool pauseKey = Application.platform == RuntimePlatform.OSXPlayer ? Input.GetKeyDown(KeyCode.JoystickButton9) : Input.GetKeyDown(KeyCode.JoystickButton7);
                if (Input.GetKeyDown(KeyCode.Escape) || pauseKey)
                    GameManager.Instance.TogglePause();
            }

            movement.SetDirection(direction);
        }
        else if (GameManager.Instance.CompareStatus(GameStatus.PAUSE))
        {
            bool pauseKey = Application.platform == RuntimePlatform.OSXPlayer ? Input.GetKeyDown(KeyCode.JoystickButton9) : Input.GetKeyDown(KeyCode.JoystickButton7);
            if (Input.GetKeyDown(KeyCode.Escape) || pauseKey)
                GameManager.Instance.TogglePause();
        }
    }
}
