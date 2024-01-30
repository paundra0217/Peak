using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private float soarMultiplier = 1.6f;
    [SerializeField] private float minimumFallTime = 0.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask PlatformLayer;

    private float soarPower;
    private bool alreadyJumped = false;
    private bool alreadySoared = false;
    private bool isFalling = false;
    private bool currentlyGrounded;
    private float fallTime;
    private Rigidbody2D rb2;
    private Collider2D cl2;
    private GroundCheck gc;
    private Animator animator;

    private void Awake()
    {
        cl2 = GetComponent<Collider2D>();
        rb2 = GetComponent<Rigidbody2D>();
        gc = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        animator = GetComponent<Animator>();
        soarPower = jumpPower * soarMultiplier;
    }

    private void Update()
    {
        bool previouslyGrounded = currentlyGrounded;
        currentlyGrounded = IsGrounded();

        if (currentlyGrounded && alreadyJumped) alreadyJumped = false;
        if (currentlyGrounded && alreadySoared) alreadySoared = false;

        if (alreadySoared)
            animator.SetBool("IsSoaring", rb2.velocity.y > 0);
        else
            animator.SetBool("IsJumping", rb2.velocity.y > 0);

        animator.SetBool("IsGrounded", currentlyGrounded);

        if (rb2.velocity.y < 0)
            fallTime += Time.deltaTime;

        if (!previouslyGrounded && currentlyGrounded)
        {
            Debug.LogFormat("Fall time: {0}", fallTime);
            Debug.LogFormat("Do Damage: {0}", fallTime > minimumFallTime);

            if (fallTime > minimumFallTime)
            {
                CalculateDamage(fallTime);
            }

            fallTime = 0;
        }
    }

    public void DoJump()
    {
        if (rb2.velocity.y < 0 || alreadyJumped) return;

        alreadyJumped = true;

        Vector2 velocity = rb2.velocity;
        velocity.y = jumpPower;
        rb2.velocity = velocity;
    }

    public void DoSoar()
    {
        if (currentlyGrounded || alreadySoared || !alreadyJumped) return;
        animator.SetBool("IsJumping", false);

        alreadySoared = true;

        Vector2 velocity = rb2.velocity;
        velocity.y = soarPower;
        rb2.velocity = velocity;
    }

    private void CalculateDamage(float velocityRate)
    {
        if (alreadySoared) return;
        print((velocityRate - minimumFallTime) / 1f * 150f);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(cl2.bounds.center, cl2.bounds.size, 0f, Vector2.down, .1f, PlatformLayer);
    }
}
