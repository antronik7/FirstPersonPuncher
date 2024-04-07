using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] int inAirJump = 1;

    private Rigidbody rb;
    private CapsuleCollider col;

    private float coyoteTimeCounter = 0;
    private int inAirJumpCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = isGrounded();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (coyoteTimeCounter > 0f)
            {
                Jump();
            }
            else if (inAirJumpCounter > 0)
            {
                --inAirJumpCounter;
                Jump();
            }
        }

        if (grounded)
        {
            coyoteTimeCounter = coyoteTime;
            inAirJumpCounter = inAirJump;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + groundCheckDistance);
    }
}

