using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
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

        Vector3 movementDirection = new Vector3(horizontal, rb.velocity.y, vertical);
        movementDirection = Vector3.ClampMagnitude(movementDirection, 1f);
        Vector3 movementVelocity = movementDirection * movementSpeed;

        rb.velocity = movementVelocity;
    }
}
