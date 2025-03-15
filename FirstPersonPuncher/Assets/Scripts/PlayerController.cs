using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    //Values
    [Header("Movement")]
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] int maxSlopeAngle = 60;
    [SerializeField] float maxSlopeSnapDistance = 1f;

    [Header("Wall Run")]
    [SerializeField] float wallMinAngle = 90f;
    [SerializeField] float wallRunningGravity = -1f;

    [Header("Camera")]
    [SerializeField] float mouseHorizontalSensitivity = 1;
    [SerializeField] float mouseVerticalSensitivity = 1;
    [SerializeField] int cameraMaxUpAngle = 90;
    [SerializeField] int cameraMaxDownAngle = -90;

    [Header("Jump")]
    [SerializeField] float jumpForce = 1f;
    [SerializeField] int inAirJump = 1;
    [SerializeField] float gravityScale = 1f;

    //References
    [Header("References")]
    [SerializeField] Transform characterOrientation;

    //Debug
    [Header("Debug")]
    [SerializeField] float currentSpeed;

    //Components
    private CapsuleCollider col;
    private Rigidbody rb;

    //Variables
    private Vector2 moveStickValues = Vector2.zero;
    private float xRotationCamera = 0f;
    private float yRotationCamera = 0f;
    [SerializeField] private bool isGrounded = false;
    private float yVelocity = 0f;
    private bool triggerJump = false;
    private int inAirJumpCounter = 0;
    private Vector3 groundNormal = Vector3.up;
    private float groundAngle = 0f;
    private Vector3 previousPosition = Vector3.zero;
    private bool isWallRunning = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Vector3.Distance(previousPosition, transform.position) / Time.deltaTime;
        previousPosition = transform.position;

        CheckForInputs();
        RotateCamera();
    }

    void LateUpdate()
    {

    }

    void FixedUpdate()
    {
        GroundCheck();
        CalculateGravity();
        ResetAirJump();
        CalculateJump();
        Move();
    }

    private void CheckForInputs()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveStickValues = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.Space))
            triggerJump = true;
    }

    private void GroundCheck()
    {
        bool wasGrounded = isGrounded;

        RaycastHit hit;
        Physics.SphereCast(transform.position + (Vector3.up * (col.height/2f)), col.radius - 0.0001f, Vector3.down, out hit, Mathf.Infinity);

        float hitNormalAngle = Vector3.Angle(hit.normal, Vector3.up);

        isGrounded = true;

        if (hit.point.y + 0.0001f < transform.position.y)
            isGrounded = false;

        if (isGrounded)
        {
            groundNormal = hit.normal;
            groundAngle = hitNormalAngle;
        }
        else
        {
            groundNormal = Vector3.up;
            groundAngle = 0f;
        }

        if (groundAngle > maxSlopeAngle + 0.0001f)
            isGrounded = false;

        if (wasGrounded && isGrounded == false && hitNormalAngle > 0.0001f && hitNormalAngle < maxSlopeAngle && !(yVelocity > 0f))
        {
            if (hit.point.y + 0.0001f > transform.position.y - maxSlopeSnapDistance)
            {
                transform.position = hit.point + (hit.normal * col.radius) - (Vector3.up * col.radius);
                isGrounded = true;
                groundNormal = hit.normal;
                groundAngle = hitNormalAngle;
            }
        }
    }

    private void CalculateGravity()
    {
        yVelocity += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;

        if (isWallRunning)
            yVelocity = wallRunningGravity;

        if (isGrounded && yVelocity < 0f)
            yVelocity = 0f;
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity;

        xRotationCamera -= mouseY;
        xRotationCamera = Mathf.Clamp(xRotationCamera, cameraMaxDownAngle, cameraMaxUpAngle);

        yRotationCamera += mouseX;

        characterOrientation.localRotation = Quaternion.Euler(0f, yRotationCamera, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotationCamera, 0f, 0f);
    }

    private void Move()
    {
        Vector3 movementDirection = characterOrientation.forward * moveStickValues.y + characterOrientation.right * moveStickValues.x;
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);
        Vector3 movementVelocity = movementDirection * movementSpeed;

        if (isGrounded && groundAngle > 0f && !(yVelocity > 0f))
            movementVelocity = Quaternion.FromToRotation(Vector3.up, groundNormal) * movementVelocity;

        Vector3 totalVelocity = movementVelocity + (Vector3.up * yVelocity);
        rb.velocity = totalVelocity;
    }

    private void ResetAirJump()
    {
        if (isGrounded)
            inAirJumpCounter = inAirJump;
    }

    private void CalculateJump()
    {
        if(triggerJump)
        {
            if (isGrounded || inAirJumpCounter > 0)
            {
                if(!isGrounded)
                    --inAirJumpCounter;

                triggerJump = false;
                yVelocity = jumpForce;
            }
            else 
            {
                triggerJump = false;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded)
        {
            float wallAngle = Vector3.Angle(collision.contacts[0].normal, Vector3.up);
            if (wallAngle >= wallMinAngle)
                isWallRunning = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (isWallRunning)
            isWallRunning = false;

        Debug.Log("Exit");
    }
}
