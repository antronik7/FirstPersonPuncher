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

    //Instances

    //Components
    private Rigidbody rb;

    //Variables
    private Vector2 moveStickValues = Vector2.zero;
    private float xRotationCamera = 0f;
    private float yRotationCamera = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

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
        Move();
    }

    private void CheckForInputs()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveStickValues = new Vector2(horizontal, vertical);
    }

    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity;

        xRotationCamera -= mouseY;
        xRotationCamera = Mathf.Clamp(xRotationCamera, cameraMaxDownAngle, cameraMaxUpAngle);

        yRotationCamera += mouseX;

        Camera.main.transform.localRotation = Quaternion.Euler(xRotationCamera, yRotationCamera, 0f);
    }

    private void Move()
    {
        Vector3 movementDirection = transform.forward * moveStickValues.y + transform.right * moveStickValues.x;
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);
        Vector3 movementVelocity = movementDirection * movementSpeed;
        rb.velocity = movementVelocity;
    }

    private void Jump()
    {

    }
}
