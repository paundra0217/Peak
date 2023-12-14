using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 20;

    private Rigidbody2D rb2;
    private GroundCheck gc;
    private float lastDirection;

    // Start is called before the first frame update
    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        gc = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        rb2.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void SetDirection(float direction)
    {
        Vector2 targetVelocity = rb2.velocity;

        //float currentSpeed = gc.IsGrounded() ? speed : speed / 2f;

        if (gc.IsGrounded() || direction != 0) lastDirection = direction;

        targetVelocity.x = lastDirection * speed;

        //if (direction > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        //else if (direction < 0) transform.rotation = Quaternion.Euler(0, 180, 0);

        rb2.velocity = Vector2.Lerp(rb2.velocity, targetVelocity, Time.deltaTime * 20f);
    }
}
