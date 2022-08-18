using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_CamPivotRotate : MonoBehaviour
{
    public float sensX = 500;
    public float sensY = 500;

    float xRotation;
    float yRotation;
    public float yRot
    {
        get { return yRotation; }
    }

    // Q를 누르면 카메라의 위치를 바꿔주고 싶다.
    // 필요속성 : 정상위치, 조준위치
    Transform mainCam;
    public Transform normalCamPosition;
    public Transform aimingCamPosition;

    public bool isAiming = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCam = Camera.main.transform;
        yRotation = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 1. Q를 누르면
        if (Input.GetKey(KeyCode.Tab))
        {
            isAiming = true;
            // 2. 카메라 위치를 aimingCamPosition으로 바꿔주고 싶다.
            mainCam.position = Vector3.Lerp(mainCam.position, aimingCamPosition.position, Time.deltaTime * 5);
            if ((mainCam.position - aimingCamPosition.position).magnitude < 0.01)
                mainCam.position = aimingCamPosition.position;
        }
        // 3. 아니면
        else
        {
            isAiming = false;
            // 4. 정상 위치를 aimingCamPosition으로 바꿔주고 싶다.
            mainCam.position = Vector3.Lerp(mainCam.position, normalCamPosition.position, Time.deltaTime * 5);
            if ((mainCam.position - normalCamPosition.position).magnitude < 0.01)
                mainCam.position = normalCamPosition.position;
        }
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20f, 85f);

        transform.eulerAngles = new Vector3(xRotation, yRotation, 0);
    }
}
