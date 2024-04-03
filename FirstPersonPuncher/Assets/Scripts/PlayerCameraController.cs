using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] Transform fpsCamera;
    [SerializeField] float mouseHorizontalSensitivity = 100;
    [SerializeField] float mouseVerticalSensitivity = 100;
    [SerializeField] int maxUpAngle = 90;
    [SerializeField] int maxDownAngle = -90;

    private float xRotation = 0f;
    private float yRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseHorizontalSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseVerticalSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxDownAngle, maxUpAngle);

        yRotation += mouseX;

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        fpsCamera.rotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
