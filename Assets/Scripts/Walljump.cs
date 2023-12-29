using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walljump : MonoBehaviour
{
    [SerializeField] private float timeToHold;

    private Rigidbody2D playerRB2;
    private GroundCheck gc;
    private bool isWalljumping;
    private float walljumpSide;

    // Start is called before the first frame update
    void Awake()
    {
        playerRB2 = gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
        gc = gameObject.transform.parent.Find("GroundCheck").GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (gc.IsGrounded()) return;

        if (other.gameObject.CompareTag("Ground"))
        {
            if (other.transform.position.x < transform.position.x)
            {
                Debug.Log("Triggered on the left side");
            }
            else if (other.transform.position.x > transform.position.x)
            {
                Debug.Log("Triggered on the right side");
            }
        }
    }


    public bool IsWalljumping()
    {
        return isWalljumping;
    }

    public float GetWalljumpSide()
    {
        return walljumpSide;
    }
}
