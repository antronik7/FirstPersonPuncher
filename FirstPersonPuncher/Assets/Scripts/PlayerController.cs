using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Values
    [Header("Movement")]
    [SerializeField] float movementSpeed = 1f;

    [Header("Camera")]
    [SerializeField] float mouseHorizontalSensitivity = 1;
    [SerializeField] float mouseVerticalSensitivity = 1;
    [SerializeField] int cameraMaxUpAngle = 90;
    [SerializeField] int cameraMaxDownAngle = -90;

    [Header("Jump")]
    [SerializeField] float jumpForce = 1f;
    [SerializeField] float gravityScale = 1f;

    //References
    [Header("References")]
    [SerializeField] Transform characterOrientation;

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
        CalculateJump();
        Move();
    }

    private void CheckForInputs()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveStickValues = new Vector2(horizontal, vertical);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            triggerJump = true;
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        Physics.SphereCast(transform.position + (Vector3.up * (col.height/2f)), col.radius - 0.0001f, Vector3.down, out hit, Mathf.Infinity);

        isGrounded = true;

        if (hit.point.y + 0.0001f < transform.position.y)
            isGrounded = false;
    }

    private void CalculateGravity()
    {
        yVelocity += Physics.gravity.y * gravityScale * Time.fixedDeltaTime;

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
        Vector3 totalVelocity = movementVelocity + (Vector3.up * yVelocity);
        rb.velocity = totalVelocity;
    }

    private void CalculateJump()
    {
        if(triggerJump)
        {
            triggerJump = false;
            yVelocity += jumpForce;
        }
    }
}
