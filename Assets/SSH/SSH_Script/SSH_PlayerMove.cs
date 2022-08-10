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
    SSH_CamPivotRotate cpr;
    public Transform body;
    public Transform camPivot;

    float yRotation;

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
        cpr = GetComponentInChildren<SSH_CamPivotRotate>();
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
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump();

        if (isJumping && Input.GetKey(KeyCode.E))
        {
            moveState = MoveState.Webbing;
            wm.isWebMove = true;
            transform.forward = body.forward;
        }
    }

    private void WebMove()
    {
        PlayerRotate(MoveState.Webbing);
        InputManage(MoveState.Webbing);
        Jump();
        //dir.y = 0;
        dir = body.forward;
        // 웹무브 시작!
        //if (isJumping && Input.GetKey(KeyCode.E))
        //{ 
        //    wm.isWebMove = true;
        //}
        if (!isJumping || Input.GetKeyUp(KeyCode.E))
        {
            moveState = MoveState.Normal;
            wm.isWebMove = false;
            webSwingEndVelocity = rb.velocity;
        }
    }

    float speed;
    float webSwingingTime = 0;
    float webSwingStartSpeed = 0;
    Vector3 webSwingEndVelocity;
    Vector3 inertiaVelocity;
    bool startFlag = false;

    void Movement()
    {
        if (!wm.isWebMove)
        {
            startFlag = true;
            speed = walkSpeed;
            webSwingingTime = 0;
        }
        else if (wm.isWebMove)
        {
            // 시작할때 한번 속력을 받고 싶다.
            if (startFlag)
            {
                webSwingStartSpeed = Math.Clamp(rb.velocity.magnitude, walkSpeed, sprintSpeed);
                startFlag = false;
            }
            webSwingingTime += Time.deltaTime;
            //speed = Mathf.Lerp(webSwingStartSpeed, 3 * webSwingStartSpeed, Time.deltaTime);
            speed = webSwingStartSpeed * (2 + webSwingingTime * 2);
        }
        
        // 웹스윙이 끝났을때의 속도를 받고 싶다.
        if (webSwingEndVelocity.magnitude < 0.1f)
            webSwingEndVelocity = Vector3.zero;
        else
        {
            webSwingEndVelocity = Vector3.Lerp(webSwingEndVelocity, Vector3.zero, Time.deltaTime * 5);
        }
        
        inertiaVelocity = webSwingEndVelocity;

        rb.velocity = dir * speed + inertiaVelocity;
        //print(speed);
    }

    Vector3 playerForward;
    void PlayerRotate(MoveState movestate)
    {
        // 거미줄 끝나고 그 방향이 지속이 안됨
        
        if (moveState == MoveState.Normal)
        {
            //  거미줄 쳤을 때
            if ((transform.up - Vector3.up).magnitude > 0.1f)
            { 
                transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.deltaTime * 10);
            }
            //평상시 걸어 다닐 때
            else
            {
                playerForward = camPivot.forward;
                playerForward.y = 0;
                body.forward = playerForward;
            }

        }
        else if (moveState == MoveState.Webbing)
        {
            if (Input.GetKey(KeyCode.E))
                transform.up = Vector3.Lerp(transform.up, wm.webDir, Time.deltaTime);
            
            float mouseX = Input.GetAxisRaw("Mouse X") * cpr.sensX * Time.deltaTime;
            yRotation += cpr.yRot;
            body.localEulerAngles = new Vector3(0, cpr.yRot, 0);
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
        IsJumping();

        yVelocity += gravity * Time.deltaTime;
        if (!isJumping || wm.isWebMove)
        {
            yVelocity = 0;
        }

        if (Input.GetButtonDown("Jump") && (!isJumping || wm.isWebMove))
        {
            yVelocity = jumpPower;
            print(1);
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
