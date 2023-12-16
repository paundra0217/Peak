using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Movement movement;
    private Jumping jump;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<Movement>();
        jump = GetComponent<Jumping>(); 
    }

    // Update is called once per frame
    void Update()
    {
        movement.SetDirection(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKey(KeyCode.Space)) jump.Jump();

        if (Input.GetKeyDown(KeyCode.Q)) jump.Soar();

        if (Input.GetKeyDown(KeyCode.I))
        {
            // Open Inventory
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Interact
        }
    }
}
