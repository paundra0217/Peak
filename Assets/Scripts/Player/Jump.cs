    using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpPower = 15f;
    [SerializeField] private float soarMultiplier = 1.6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask PlatformLayer;

    private float soarPower;
    private bool alreadyJumped = false;
    private bool alreadySoared = false;
    private bool currentlyGrounded;
    private float fallTime;
    private Rigidbody2D rb2;
    //private Collider2D cl2;
    private GroundCheck gc;
    private Animator animator;
    private PlayerHealth playerhealth;
    private PlayerStamina playerstamina;

    private void Awake()
    {
        //cl2 = GetComponent<Collider2D>();
        rb2 = GetComponent<Rigidbody2D>();
        gc = transform.Find("GroundCheck").GetComponent<GroundCheck>();
        animator = GetComponent<Animator>();
        playerhealth = GetComponent<PlayerHealth>();
        playerstamina = GetComponent<PlayerStamina>();
        soarPower = jumpPower * soarMultiplier;
    }

    private void Update()
    {
        //HandleFallDamage();
        currentlyGrounded = gc.IsGrounded();

        if (currentlyGrounded && alreadyJumped) alreadyJumped = false;
        if (currentlyGrounded && alreadySoared) alreadySoared = false;

        if (alreadySoared)
            animator.SetBool("IsSoaring", rb2.velocity.y > 0);
        else
            animator.SetBool("IsJumping", rb2.velocity.y > 0);

        animator.SetBool("IsGrounded", currentlyGrounded);
    }

    //private void HandleFallDamage()
    //{
    //    bool previouslyGrounded = currentlyGrounded;
    //    currentlyGrounded = gc.IsGrounded();

    //    if (rb2.velocity.y < -2f && !currentlyGrounded)
    //        fallTime += Time.deltaTime;

    //    if (!previouslyGrounded && currentlyGrounded)
    //    {
    //        if (!alreadySoared)
    //        {
    //            CalculateDamage(fallTime);
    //            AudioController.Instance.PlaySFX("Land");
    //        }

    //        fallTime = 0;
    //    }
    //}

    public void DoJump()
    {
        if (rb2.velocity.y < 0 || alreadyJumped) return;

        alreadyJumped = true;
        fallTime = 0;

        AudioController.Instance.PlaySFX("Jump");
        playerstamina.DepleteStamina();
        Vector2 velocity = rb2.velocity;
        velocity.y = jumpPower;
        rb2.velocity = velocity;
    }

    public void DoSoar()
    {
        if (currentlyGrounded || alreadySoared || !alreadyJumped) return;
        animator.SetBool("IsJumping", false);

        alreadySoared = true;
        fallTime = 0;

        AudioController.Instance.PlaySFX("Soar");
        playerstamina.DepleteStamina();
        Vector2 velocity = rb2.velocity;
        velocity.y = soarPower;
        rb2.velocity = velocity;
    }

    //private void CalculateDamage(float fallTime)
    //{
    //    if (alreadySoared) return;
    //    float damageTaken = fallTime / 0.1f * 150f;
    //    print(damageTaken);

    //    playerhealth.TakeDamage(damageTaken);
    //}

    //public bool IsGrounded()
    //{
    //    return Physics2D.BoxCast(cl2.bounds.center, cl2.bounds.size, 0f, Vector2.down, .25f, PlatformLayer);
    //}
}
