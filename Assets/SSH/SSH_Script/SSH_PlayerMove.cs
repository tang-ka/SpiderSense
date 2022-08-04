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
    }

    void Update()
    {
        StateManage();
        Movement();
    }

    private void StateManage()
    {
        if (state == PlayerState.Normal)
        {
            NormalInput();
        }
        else if (state == PlayerState.Webbing)
        {
            WebbingInput();
        }
    }

    private void WebbingInput()
    {
        Rotate();

        float v = Input.GetAxisRaw("Vertical");

        dir = transform.forward * v;
        dir.Normalize();

        Jump();
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

    void Movement()
    {
        rb.velocity = dir * walkSpeed;
    }

    // 플레이어가 보는 방향을 카메라의 방향과 맞춰준다.
    void Rotate()
    {
        Vector3 playerForward = camDir.forward;
        playerForward.y = 0;

        transform.forward = playerForward;
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

        Debug.DrawRay(ray.origin, ray.direction * 1.1f, Color.red);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            
            if (hitInfo.distance < 1.1f && yVelocity <= 0)
            {
                isJumping = false;
            }
            else
                isJumping = true;
        }
        else
            isJumping = true;
    }
}
