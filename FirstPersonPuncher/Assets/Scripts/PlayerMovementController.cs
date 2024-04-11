using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Transform characterOrientation;
    [SerializeField] float movementSpeed = 1f;

    private Rigidbody rb;
    private JumpController jumpController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpController = GetComponent<JumpController>();
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = characterOrientation.forward * vertical + characterOrientation.right * horizontal;
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);
        Vector3 movementVelocity = movementDirection * movementSpeed;

        if (jumpController.getIsGrounded() && jumpController.getGroundAngle() > 0f)
            movementVelocity = Quaternion.FromToRotation(Vector3.up, jumpController.getGroundNormal()) * movementVelocity;

        movementVelocity.y += jumpController.getYVelocity();

        rb.velocity = movementVelocity;
    }
}
