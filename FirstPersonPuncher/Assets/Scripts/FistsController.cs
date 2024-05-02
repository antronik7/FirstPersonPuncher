using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistsController : MonoBehaviour
{
    //Values
    [Header("Punch")]
    [SerializeField] float punchSpeed = 0.2f;
    [SerializeField] float punchCooldown = 0.5f;
    [SerializeField] Vector3 punchPosition = Vector3.zero;

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
        if(punchLeft)
            currentFist = leftFist;

        float timeCounter = 0f;
        while (currentFist.localPosition != punchPosition) 
        {
            currentFist.localPosition = Vector3.Lerp(currentFist.localPosition, punchPosition, timeCounter/punchSpeed);
            yield return 0;
            timeCounter += Time.deltaTime;
        }
    }
}
