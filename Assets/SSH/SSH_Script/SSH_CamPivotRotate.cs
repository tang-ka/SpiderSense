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

    // Q�� ������ ī�޶��� ��ġ�� �ٲ��ְ� �ʹ�.
    // �ʿ�Ӽ� : ������ġ, ������ġ
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
        // 1. Q�� ������
        if (Input.GetKey(KeyCode.Tab))
        {
            isAiming = true;
            // 2. ī�޶� ��ġ�� aimingCamPosition���� �ٲ��ְ� �ʹ�.
            mainCam.position = Vector3.Lerp(mainCam.position, aimingCamPosition.position, Time.deltaTime * 5);
            if ((mainCam.position - aimingCamPosition.position).magnitude < 0.01)
                mainCam.position = aimingCamPosition.position;
        }
        // 3. �ƴϸ�
        else
        {
            isAiming = false;
            // 4. ���� ��ġ�� aimingCamPosition���� �ٲ��ְ� �ʹ�.
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
