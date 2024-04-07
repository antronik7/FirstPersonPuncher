using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] int inAirJump = 1;

    private Rigidbody rb;
    private CapsuleCollider col;

    private bool isGrounded = false;
    private float yVelocity = 0f;
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
        CheckGround();
        ApplyGravity();

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

        CheckCoyoteTime();
    }

    private void Jump()
    {
        yVelocity = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight);
    }

    private void CheckGround()
    {
        RaycastHit hit;
        isGrounded = Physics.SphereCast(transform.position, col.radius, Vector3.down, out hit, ((col.height / 2f) - col.radius) + groundCheckDistance);
    }

    private void ApplyGravity()
    {
        yVelocity += Physics.gravity.y * Time.deltaTime;

        if (isGrounded && yVelocity < 0f)
            yVelocity = 0f;

        rb.velocity = new Vector3(rb.velocity.x, yVelocity, rb.velocity.z);
    }

    private void CheckCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            inAirJumpCounter = inAirJump;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
}

