using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] Transform characterOrientation;
    [SerializeField] float movementSpeed = 1f;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = characterOrientation.forward * vertical + characterOrientation.right * horizontal;
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);
        Vector3 movementVelocity = movementDirection * movementSpeed;
        Vector3 totalVelocity = new Vector3(movementVelocity.x, rb.velocity.y, movementVelocity.z);

        rb.velocity = totalVelocity;
    }
}
