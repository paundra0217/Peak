using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour
{
    [SerializeField] private float jumpPower = 20;

    private float soarPower;
    private bool alreadySoared = false;
    private Rigidbody2D rb2;
    private GroundCheck gc;

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        gc = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        soarPower = jumpPower * 2;
    }

    private void Update()
    {
        if(gc.IsGrounded() && alreadySoared) alreadySoared = false;
    }

    public void Jump()
    {
        if (!gc.IsGrounded()) return;

        Vector2 velocity = rb2.velocity;
        velocity.y = jumpPower;
        rb2.velocity = velocity;
    }

    public void Soar()
    {
        if (gc.IsGrounded() || alreadySoared) return;

        alreadySoared = true;

        Vector2 velocity = rb2.velocity;
        velocity.y = soarPower;
        rb2.velocity = velocity;
    }
}
