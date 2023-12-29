using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpPower = 15f;

    private float soarPower;
    private bool alreadyJumped = false;
    private bool alreadySoared = false;
    private Rigidbody2D rb2;
    private GroundCheck gc;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        gc = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        soarPower = jumpPower * 2f;
    }

    private void Update()
    {
        if(gc.IsGrounded() && alreadyJumped) alreadyJumped = false;
        if(gc.IsGrounded() && alreadySoared) alreadySoared = false;
    }

    public void DoJump()
    {
        if (!gc.IsGrounded()) return;

        alreadyJumped = true;

        Vector2 velocity = rb2.velocity;
        velocity.y = jumpPower;
        rb2.velocity = velocity;
    }

    public void DoSoar()
    {
        if (gc.IsGrounded() || alreadySoared || !alreadyJumped) return;

        alreadySoared = true;

        Vector2 velocity = rb2.velocity;
        velocity.y = soarPower;
        rb2.velocity = velocity;
    }

    static public void TriggerWalljump()
    {

    }
}
