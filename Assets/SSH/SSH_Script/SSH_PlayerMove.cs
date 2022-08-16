using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 일반적인 입력을 받아 이동하고 싶다.
public class SSH_PlayerMove : MonoBehaviour
{
    public static SSH_PlayerMove Instance;
    private void Awake()
    {
        Instance = this;
    }

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
    private float yVelocity = 0;
    public float jumpPower = 5f;
    //private bool isJumping;
    float jumpRayLen = 1.3f;

    float speed;
    float webSwingingTime = 0;
    float webSwingStartSpeed = 0;
    Vector3 webSwingEndVelocity;
    Vector3 inertiaVelocity;
    public float webJumpFactor = 3;
    bool startFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wm = GetComponent<SSH_WebMove>();
        cpr = GetComponentInChildren<SSH_CamPivotRotate>();
    }

    #region MoveState
    public enum MoveState
    {
        Normal,
        Floating,
        WebSwing,
        WebZip,
        PointWebZip,
        PointLaunch,
        WallRunVertical,
        WallRunHorizontal,
    }
    public MoveState moveState = MoveState.Normal;
    #endregion

    // Update is called once per frame
    void Update()
    {
        switch (moveState)
        {
            case MoveState.Normal:
                Normal();
                break;

            case MoveState.Floating:
                Floating();
                break;

            case MoveState.WebSwing:
                WebSwing();
                break;

            case MoveState.PointWebZip:
                PointWebZip();
                break;

            case MoveState.PointLaunch:
                PointLaunch();
                break;

            case MoveState.WallRunVertical:
                WallRunVertical();
                break;

            case MoveState.WallRunHorizontal:
                WallRunHorizontal();
                break;
        }

        Movement();
    }

    float currentTime;
    public float offsetTime = 1;
    float webJumpForce = 500;
    bool isWebJump = false;
    /******************************** State Method ***********************************/
    private void Normal()
    {
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump(MoveState.Normal);

        // 점프하면 Floating 상태로 전환하고 싶다. 
        if (IsJumping())
        {
            moveState = MoveState.Floating;
        }
    }

    private void Floating()
    {
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump(MoveState.Floating);

        // 웹스윙 시작!
        if (!IsJumping())
        {
            moveState = MoveState.Normal;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            wm.isGoWebSwing = true;
            moveState = MoveState.WebSwing;
            transform.forward = body.forward;
        }
        // Web Zip 시작
        else if (Input.GetButtonDown("Jump"))
        {
            wm.isGoWebZip = true;
        }
    }

    private void WebSwing()
    {
        PlayerRotate(MoveState.WebSwing);
        InputManage(MoveState.WebSwing);
        Jump(MoveState.WebSwing);
        dir = body.forward;

        if (Input.GetButtonDown("Jump"))
        {
            wm.isClickE = false;
            wm.isGoWebSwing = false;
            moveState = MoveState.Floating;
            webSwingEndVelocity = rb.velocity * webJumpFactor;
            isWebJump = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            yVelocity = 0;
            wm.isGoWebSwing = false;
            moveState = MoveState.Floating;
            webSwingEndVelocity = rb.velocity;
        }


    }

    private void WebZip()
    {
        print("Web Zip!!!");
        currentTime += Time.deltaTime;
        if (currentTime > 1)
        {
            wm.isGoWebSwing = false;
            moveState = MoveState.Floating;
        }
    }

    private void PointWebZip()
    {
        throw new NotImplementedException();
    }

    private void PointLaunch()
    {
        throw new NotImplementedException();
    }

    private void WallRunVertical()
    {
        throw new NotImplementedException();
    }

    private void WallRunHorizontal()
    {
        throw new NotImplementedException();
    }
    /*********************************************************************************/

    void Movement()
    {
        if (moveState == MoveState.Normal)
        {
            startFlag = true;
            speed = walkSpeed;
            webSwingEndVelocity /= 1.5f; // 마찰력 + 정지하려는 힘 -> 관성 확 줄이기
        }
        else if (moveState == MoveState.Floating)
        {
            startFlag = true;
            speed = walkSpeed;
            webSwingingTime = 0;
        }
        else if (moveState == MoveState.WebSwing)
        {
            // 시작할때 한번 속력을 받고 싶다.
            if (startFlag)
            {
                webSwingStartSpeed = Math.Clamp(rb.velocity.magnitude, walkSpeed, sprintSpeed);
                startFlag = false;
            }
            webSwingingTime += Time.deltaTime;
            speed = webSwingStartSpeed * (2 + webSwingingTime * 2);
        }
        
        // 웹스윙이 끝났을때의 속도를 받고 싶다.
        if (webSwingEndVelocity.magnitude < 0.1f)
        {
            webSwingEndVelocity = Vector3.zero;
            isWebJump = false;
        }
        else if (isWebJump)
        {
            webSwingEndVelocity = Vector3.Lerp(webSwingEndVelocity, Vector3.zero, Time.deltaTime * 1.5f);
        }
        else
        {
            webSwingEndVelocity = Vector3.Lerp(webSwingEndVelocity, Vector3.zero, Time.deltaTime * 4);
        }

        inertiaVelocity = webSwingEndVelocity;

        rb.velocity = dir * speed + inertiaVelocity;
    }

    Vector3 playerForward;
    void PlayerRotate(MoveState state)
    {
        if (state == MoveState.Normal)
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
        else if (state == MoveState.WebSwing)
        {
            if (Input.GetKey(KeyCode.E))
                transform.up = Vector3.Lerp(transform.up, wm.webDir, Time.deltaTime * 0.3f);
            
            float mouseX = Input.GetAxisRaw("Mouse X") * cpr.sensX * Time.deltaTime;
            yRotation += cpr.yRot;
            body.localEulerAngles = new Vector3(0, cpr.yRot, 0);
        }
    }

    void InputManage(MoveState state)
    {
        if (state == MoveState.Normal)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            dir = body.forward * v + body.right * h;
            dir.Normalize();
        }
        else if (state == MoveState.WebSwing)
        {
            float v = Input.GetAxisRaw("Vertical");

            dir = body.forward * v;
            dir.Normalize();
        }
    }
    
    void Jump(MoveState state)
    {
        if (state == MoveState.Normal)
        {
            yVelocity = 0;

            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
        }
        else if (state == MoveState.Floating)
        {
            yVelocity += gravity * Time.deltaTime;
        }
        else if (state == MoveState.WebSwing)
        {
            yVelocity = 0;
        }

        dir.y = yVelocity;
        //print(yVelocity);
    }

    public bool IsJumping()
    {
        Ray ray = new Ray(body.position, -body.up);
        RaycastHit hitInfo;
        Debug.DrawRay(ray.origin, ray.direction * jumpRayLen, Color.red);

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.distance < jumpRayLen && yVelocity <= 0)
                return false;
            else
                return true;
        }
        else
            return true;
    }
}
