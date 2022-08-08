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
        NormalMove,
        WebMove
    }
    public MoveState moveState = MoveState.NormalMove;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        StateManage();
    }

    private void StateManage()
    {
        switch (moveState)
        {
            case MoveState.NormalMove:
                PlayerRotate();
                InputManage(MoveState.NormalMove);
                Jump();
                break;

            case MoveState.WebMove:
                PlayerRotate();
                InputManage(MoveState.WebMove);
                Jump();
                break;
        }

        Movement();
    }

    void Movement()
    {
        rb.velocity = dir * walkSpeed;
    }

    void InputManage(MoveState movestate)
    {
        if (moveState == MoveState.NormalMove)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            dir = body.forward * v + body.right * h;
            dir.Normalize();
        }
        else if (moveState == MoveState.WebMove)
        {
            float v = Input.GetAxisRaw("Vertical");

            dir = body.forward * v;
            dir.Normalize();
        }
    }

    void PlayerRotate()
    {
        Vector3 playerForward = camPivot.forward;
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
