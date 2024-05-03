using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsController : MonoBehaviour
{
    //Values
    [Header("Punch")]
    [SerializeField] float punchSpeed = 0.2f;
    [SerializeField] float punchImpactDuration = 0.2f;
    [SerializeField] float punchCooldown = 0.5f;
    [SerializeField] Vector3 rightFinalPunchPosition = Vector3.zero;
    [SerializeField] Vector3 leftFinalPunchPosition = Vector3.zero;

    //References
    [Header("References")]
    [SerializeField] Transform leftFist;
    [SerializeField] Transform rightFist;

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

        if (punchLeft)
        {
            currentFist = leftFist;
            currrentPosition = leftFistOriginalPosition;
            punchPosition = leftFinalPunchPosition;
        }

        punchLeft = !punchLeft;

        float timeCounter = 0f;
        while (currentFist.localPosition != punchPosition) 
        {
            float ratio = timeCounter / punchSpeed;
            currentFist.localPosition = Vector3.Lerp(currrentPosition, punchPosition, ratio);
            yield return 0;
            timeCounter += Time.deltaTime;
        }

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
}
