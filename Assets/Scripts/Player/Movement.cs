using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb2;
    private PlayerSpeed speed;
    private GroundCheck gc;
    private Animator animator;
    private SpriteRenderer sr;
    private float direction;
    private float lastDirection;

    // Start is called before the first frame update
    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        speed = GetComponent<PlayerSpeed>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gc = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        rb2.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetDirection(float direction)
    {
        //float currentSpeed = gc.IsGrounded() ? speed : speed / 2f;

        //if (gc.IsGrounded() || direction != 0) lastDirection = direction;

        //if (direction > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        //else if (direction < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

        Vector2 Vec2D0 = Vector2.zero;

        if (GameManager.Instance.CompareStatus(GameStatus.TRANSITION))
        {
            rb2.constraints = RigidbodyConstraints2D.FreezePosition;
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(direction));
            float xVel = direction * speed.GetSpeed();

            // Flip sprite
            if (direction != 0)
                sr.flipX = direction < 0f;

            Vector2 targetVelocity = new Vector2(xVel, rb2.velocity.y);
            rb2.velocity = Vector2.SmoothDamp(rb2.velocity, targetVelocity, ref Vec2D0, 0.015f);
        }
    }
}
