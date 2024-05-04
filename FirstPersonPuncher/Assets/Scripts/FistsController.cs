using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsController : MonoBehaviour
{
    //Values
    [Header("Punch")]
    [SerializeField] float punchForce = 1f;
    [SerializeField] float punchSpeed = 0.2f;
    [SerializeField] float punchImpactDuration = 0.2f;
    [SerializeField] float punchCooldown = 0.5f;
    [SerializeField] Vector3 rightFinalPunchPosition = Vector3.zero;
    [SerializeField] Vector3 leftFinalPunchPosition = Vector3.zero;

    //References
    [Header("References")]
    [SerializeField] Transform leftFist;
    [SerializeField] Transform rightFist;
    [SerializeField] Transform characterOrientation;

    //Components
    private SphereCollider rightFistCol;
    private SphereCollider leftFistCol;

    //Variables
    private bool punchLeft = true;
    private float punchCooldownCounter = 0;
    private Vector3 leftFistOriginalPosition;
    private Vector3 rightFistOriginalPosition;
    private Quaternion leftFistOriginalRotation;
    private Quaternion rightFistOriginalRotation;

    private void Awake()
    {
        leftFistOriginalPosition = leftFist.localPosition;
        rightFistOriginalPosition = rightFist.localPosition;
        leftFistOriginalRotation = leftFist.localRotation;
        rightFistOriginalRotation = rightFist.localRotation;
        leftFistCol = leftFist.GetComponent<SphereCollider>();
        rightFistCol = rightFist.GetComponent<SphereCollider>();
}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        punchCooldownCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1"))
            Punch();
    }

    private void Punch()
    {
        if (punchCooldownCounter > 0f)
            return;

        punchCooldownCounter = punchCooldown;

        StartCoroutine(PunchAnimation());
    }

    IEnumerator PunchAnimation()
    {
        Transform currentFist = rightFist;
        Vector3 currrentPosition = rightFistOriginalPosition;
        Vector3 punchPosition = rightFinalPunchPosition;
        SphereCollider col = rightFistCol;

        if (punchLeft)
        {
            currentFist = leftFist;
            currrentPosition = leftFistOriginalPosition;
            punchPosition = leftFinalPunchPosition;
            col = leftFistCol;
        }

        punchLeft = !punchLeft;

        col.enabled = true;

        float timeCounter = 0f;
        while (currentFist.localPosition != punchPosition) 
        {
            float ratio = timeCounter / punchSpeed;
            currentFist.localPosition = Vector3.Lerp(currrentPosition, punchPosition, ratio);
            yield return 0;
            timeCounter += Time.deltaTime;
        }

        col.enabled = false;
        yield return new WaitForSeconds(punchImpactDuration);

        timeCounter = 0f;
        while (currentFist.localPosition != currrentPosition)
        {
            float ratio = timeCounter / punchSpeed;
            currentFist.localPosition = Vector3.Lerp(punchPosition, currrentPosition, ratio);
            yield return 0;
            timeCounter += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.GetComponent<Rigidbody>();
        if (otherRb != null) 
        {
            otherRb.AddForceAtPosition(characterOrientation.forward * punchForce, other.ClosestPointOnBounds(transform.position));
        }
    }
}
