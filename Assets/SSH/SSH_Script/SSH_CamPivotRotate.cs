using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPivotRotate : MonoBehaviour
{
    public float sensX = 500;
    public float sensY = 500;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 85f);

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }
}
