using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 일반적인 입력을 받아 이동하고 싶다.
public class SSH_PlayerMove : MonoBehaviour
{
    public float walkSpeed = 8;
    public float runSpeed = 10;
    public float sprintSpeed = 15;
    Vector3 dir;

    Rigidbody rb;
    SSH_WebMove wm;
    public Transform body;
    public Transform camPivot;

    float gravity = -9.81f;
    public float yVelocity = 0;
    public float jumpPower = 5f;
    public bool isJumping;
    float jumpRayLen = 1.2f;

    #region MoveState
    public enum MoveState
    {
        Normal,
        Webbing
    }
    public MoveState moveState = MoveState.Normal;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wm = GetComponent<SSH_WebMove>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (moveState)
        {
            case MoveState.Normal:
                NormalMove();
                break;

            case MoveState.Webbing:
                WebMove();
                break;
        }

        Movement();
    }

    private void NormalMove()
    {
        wm.isWebMove = false;
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump();

        if (isJumping && Input.GetKey(KeyCode.E))
        {
            moveState = MoveState.Webbing;
        }
    }

    private void WebMove()
    {
        PlayerRotate(MoveState.Webbing);
        InputManage(MoveState.Webbing);
        Jump();

        if (isJumping)
            wm.isWebMove = true;
        else
        {
            moveState = MoveState.Normal;
        }
    }

    void Movement()
    {
        rb.velocity = dir * walkSpeed;
    }

    void PlayerRotate(MoveState movestate)
    {
        if (moveState == MoveState.Normal)
        {
            transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * 5);

            Vector3 playerForward = camPivot.forward;
            playerForward.y = 0;

            body.forward = playerForward;
        }
        else if (moveState == MoveState.Webbing)
        {
            if (Input.GetKey(KeyCode.E))
                transform.up = Vector3.Lerp(transform.up, wm.webDir, Time.deltaTime);
            else
                transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * 5);
        }
    }

    void InputManage(MoveState movestate)
    {
        if (moveState == MoveState.Normal)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            dir = body.forward * v + body.right * h;
            dir.Normalize();
        }
        else if (moveState == MoveState.Webbing)
        {
            float v = Input.GetAxisRaw("Vertical");

            dir = body.forward * v;
            dir.Normalize();
        }
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
        Ray ray = new Ray(body.position, -body.up);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * jumpRayLen, Color.red);

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.distance < jumpRayLen && yVelocity <= 0)
                isJumping = false;
            else
                isJumping = true;
        }
        else
            isJumping = true;
    }
}
