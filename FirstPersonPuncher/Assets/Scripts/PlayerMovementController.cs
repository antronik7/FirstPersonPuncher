using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Transform characterOrientation;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float maxAutoStepHeight = 0.5f;
    [SerializeField] float stepSpeed = 1f;

    private Rigidbody rb;
    private CapsuleCollider col;
    private JumpController jumpController;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        jumpController = GetComponent<JumpController>();
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = characterOrientation.forward * vertical + characterOrientation.right * horizontal;
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);

        AutoStep(movementDirection);

        Vector3 movementVelocity = movementDirection * movementSpeed;

        if (jumpController.getIsGrounded() && jumpController.getGroundAngle() > 0f)
            movementVelocity = Quaternion.FromToRotation(Vector3.up, jumpController.getGroundNormal()) * movementVelocity;

        movementVelocity.y += jumpController.getYVelocity();

        rb.velocity = movementVelocity;
    }

    private void AutoStep(Vector3 direction)
    {
        direction.Normalize();
        RaycastHit hit;
        if (Physics.Raycast(transform.position - (Vector3.up * (col.height/2f - 0.01f)), direction, out hit, col.radius + 0.01f))
            if (Physics.Raycast(transform.position - (Vector3.up * (col.height / 2f - maxAutoStepHeight)) + (direction * (col.radius + 0.01f)), Vector3.down, out hit, maxAutoStepHeight - 0.01f))
                rb.position += new Vector3 (0f, stepSpeed * Time.deltaTime, 0f);
    }
}
