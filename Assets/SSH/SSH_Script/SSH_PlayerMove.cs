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
    Animator anim;
    SSH_WebMove wm;
    SSH_CamPivotRotate cpr;
    public Transform body;
    public Transform camPivot;

    float yRotation;

    float gravity = -9.81f;
    private float yVelocity = 0;
    public float jumpPower = 5f;
    //private bool isJumping;
    public float jumpRayLen = 2.3f;

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
        anim = GetComponentInChildren<Animator>();
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
        KeyClickManager();
        Debug.DrawLine(transform.position, transform.position - (transform.up * 5), Color.black);


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

            case MoveState.WebZip:
                WebZip();
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
    bool isWebJump = false;
    /******************************** State Method ***********************************/
    private void Normal()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime * (FOVChangeSpeed - 1));
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump(MoveState.Normal);

        // 점프하면 Floating 상태로 전환하고 싶다. 
        if (IsJumping())
        {
            moveState = MoveState.Floating;
            anim.SetTrigger("Jump");
        }
        
        if (isClickMouse2)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                wm.isGoPointWebZip = true;
                wm.pointWebZipFlag = true;
                wm.isFinishSkill = false;
                moveState = MoveState.PointWebZip;
            }
        }
    }

    private void Floating()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime * (FOVChangeSpeed - 1));
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump(MoveState.Floating);

        if (!IsJumping())
        {
            moveState = MoveState.Normal;
            anim.SetTrigger("Idle");
        }
        // Point Web Zip 시작
        else if (isClickMouse2)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                wm.isGoPointWebZip = true;
                wm.pointWebZipFlag = true;
                wm.isFinishSkill = false;
                moveState = MoveState.PointWebZip;
            }
        }
        // Web Swing 시작
        else if (Input.GetKeyDown(KeyCode.E))
        {
            wm.isGoWebSwing = true;
            wm.webSwingFlag = true;
            moveState = MoveState.WebSwing;
            transform.forward = body.forward;
        }

        // Web Zip 시작
        if (Input.GetButtonDown("Jump"))
        {
            print("두번 점프 하는거다");
            wm.isGoWebZip = true;
            wm.webZipFlag = true;
            moveState = MoveState.WebZip;
            //webSwingEndVelocity = (transform.forward + Vector3.up * 0.5f) * webZipFactor;
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

    float webZipSpeed = 130;

    private void WebZip()
    {
        PlayerRotate(MoveState.Normal);
        Jump(MoveState.WebZip);
        // Web Zip 시작
        currentTime += Time.deltaTime;

        Vector3 webZipDir = body.forward + body.up * 0.45f;
        webZipDir.Normalize();

        webZipSpeed = Mathf.Lerp(webZipSpeed, 20, Time.deltaTime * 2.5f);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 100, Time.deltaTime * FOVChangeSpeed);

        rb.velocity = webZipDir * webZipSpeed;

        //print(webZipDir + ", " + rb.velocity + ", " + rb.velocity.magnitude);
        // Web Zip 끝
        if (currentTime > 0.5f)
        {
            wm.isGoWebZip = false;
            wm.isFinishSkill = true;
            moveState = MoveState.Floating;
            webSwingEndVelocity = rb.velocity;

            webZipSpeed = 180;
            currentTime = 0;
        }
        // -> 현재 문제 rb.velocity값이 왔다갔다함. (정상값, (0, 1, 0)의 스케일값, 크기가 작은 값)
    }

    public Vector3 reachPoint;
    float initialFOV = 60;
    float FOVChangeSpeed = 5;

    private void PointWebZip()
    {
        Jump(MoveState.PointWebZip);
        currentTime += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, reachPoint, Time.deltaTime * 5);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 100, Time.deltaTime * FOVChangeSpeed);

        //print("currentTime : " + currentTime);
        if (Vector3.Distance(transform.position, reachPoint) < 2)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            //if (isClickJump)
            //{
            //    moveState = MoveState.PointLaunch;
            //}

            if (Physics.Raycast(ray, out hit, 2f))
            {
                wm.isGoPointWebZip = false;
                wm.isFinishSkill = true;
                print("지우자");
                moveState = MoveState.Normal;

                currentTime = 0;
            }
        }
    }

    private void PointLaunch()
    {
        Jump(MoveState.PointWebZip);

        transform.position = Vector3.Lerp(transform.position, reachPoint, Time.deltaTime * 5);
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        // 도착하면
        if (Vector3.Distance(transform.position, reachPoint) < 2)
        {
            if (Physics.Raycast(ray, out hit, 3f))
            {
                Vector3 launchDir = wm.offsetDir + Vector3.up * 0.5f;

                //webSwingEndVelocity = launchDir * 5;
                //wm.isGoPointWebZip = false;
                //wm.isFinishSkill = true;
                //moveState = MoveState.Floating;

                currentTime = 0;
            }
        }
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
            webSwingEndVelocity = Vector3.Lerp(webSwingEndVelocity, Vector3.zero, Time.deltaTime);
        }

        inertiaVelocity = webSwingEndVelocity;

        //print(inertiaVelocity);
        if (moveState != MoveState.WebZip)
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

            if (v > 0)
                anim.SetTrigger("Walk");
            else
                anim.SetTrigger("Idle");

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
        else if (state == MoveState.WebSwing || state == MoveState.WebZip || state == MoveState.PointWebZip)
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

    bool isClickMouse2 = false;
    bool isClickJump = false;

    void KeyClickManager()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isClickMouse2 = true;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            isClickMouse2 = false;
            currentTime = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            isClickJump = true;
        }
        if (isClickJump)
        {
            float currentTime = 0;
            currentTime += Time.deltaTime;
            if (currentTime > 1)
            {
                isClickJump = false;
            }
        }

        
    }
}   
