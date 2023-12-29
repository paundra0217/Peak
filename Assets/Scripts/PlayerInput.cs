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
        if (GameManager.Instance.GetStatus() == GameStatus.DIALOG)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("Next Story Please");
            }
        } 
        else
        {
            movement.SetDirection(Input.GetAxisRaw("Horizontal"));

            if (Input.GetKey(KeyCode.Space)) jump.DoJump();

            if (Input.GetKeyDown(KeyCode.Q)) jump.DoSoar();

            if (Input.GetKeyDown(KeyCode.I))
            {
                // Open Inventory
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Interact
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.TestFunction();
            }
        }
    }
}
