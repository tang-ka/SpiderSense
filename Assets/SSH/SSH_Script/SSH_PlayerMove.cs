using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력을 받아 이동하고 싶다.
public class SSH_PlayerMove : MonoBehaviour
{
    public float walkSpeed = 10;
    Vector3 dir = Vector3.zero;

    Rigidbody rb;
    SSH_SpiderMove spiderMovement;
    SSH_CamPivotRotate cpr;
    float yRotation;
    public Transform camDir;

    float gravity = -9.81f;
    public float yVelocity = 0;
    public float jumpPower = 5f;
    public bool isJumping;

    #region Player state
    public enum PlayerState
    {
        Normal,
        Webbing
    }
    public PlayerState state = PlayerState.Normal;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spiderMovement = GetComponent<SSH_SpiderMove>();
        cpr = GetComponentInChildren<SSH_CamPivotRotate>();
    }

    void Update()
    {
        StateManage();
    }

    private void StateManage()
    {
        if (state == PlayerState.Normal)
        {
            Movement();
            if (Input.GetKey(KeyCode.E))
                state = PlayerState.Webbing;
        }
        else if (state == PlayerState.Webbing)
        {
            WebSwing();
            if (Input.GetKeyUp(KeyCode.E))
                state = PlayerState.Normal;
        }
    }

    public Transform hook;
    public Transform rightHand;

    Vector3 defaultHookPosition;
    Vector3 grapHookPosition;

    public Transform body;
    private void WebSwing()
    {
        //transform.forward = camDir.forward;

        if (Input.GetKey(KeyCode.E))
        {
            Vector3 webDir = hook.position - rightHand.position;
            float webLen = Vector3.Distance(rightHand.position, hook.position);

            Ray web = new Ray(rightHand.position, webDir);
            Debug.DrawRay(web.origin, web.direction * webLen, Color.white);

            RaycastHit pivot;
            if (Physics.Raycast(web, out pivot, webLen))
            {
                Debug.DrawRay(transform.position, transform.up * webLen, Color.red);
                // 거미줄 
                transform.up = web.direction;

                float mouseX = Input.GetAxisRaw("Mouse X") * cpr.sensX * Time.deltaTime;
                yRotation += mouseX;
                body.localEulerAngles = new Vector3(0, yRotation, 0);

                //float theta = Vector3.Angle(-transform.up, Vector3.down);
                //dir = transform.forward + transform.up;
                //float speed = gravity * Mathf.Sin(theta * Mathf.Deg2Rad);

                //float v = Input.GetAxisRaw("Vertical");

                //dir = transform.forward * v + transform.up;
                //dir.Normalize();
            }
        }

        Jump();

        rb.velocity = dir * walkSpeed;

        if (Input.GetKeyUp(KeyCode.E))
        {
            transform.up = Vector3.up;
        }
    }

    void Movement()
    {
        Rotate();

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = body.forward * v + body.right * h;
        dir.Normalize();

        Jump();

        rb.velocity = dir * walkSpeed;
    }

    // 플레이어가 보는 방향을 카메라의 방향과 맞춰준다.
    void Rotate()
    {
        Vector3 playerForward = camDir.transform.forward;
        playerForward.y = 0;

        body.forward = playerForward;
    }

    void Jump()
    {
        yVelocity += gravity * Time.deltaTime;
        if (!isJumping)
        {
            yVelocity = 0;
        }
        IsJumping();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        dir.y = yVelocity;
    }

    void IsJumping()
    {
        Ray ray = new Ray(transform.position + transform.up, -transform.up);

        Debug.DrawRay(ray.origin, ray.direction * 1.2f, Color.red);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            
            if (hitInfo.distance < 1.2f && yVelocity <= 0)
            {
                isJumping = false;
            }
            else
                isJumping = true;
        }
        else
            isJumping = true;
    }

    private void NormalInput()
    {
        // 일반적인 입력을 받아 이동하고 싶다.
        Rotate();

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = transform.forward * v + transform.right * h;
        dir.Normalize();

        Jump();
    }
}
