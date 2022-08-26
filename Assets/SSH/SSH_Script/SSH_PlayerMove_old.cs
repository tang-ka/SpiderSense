using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 일반적인 입력을 받아 이동하고 싶다.
public class SSH_PlayerMove_old : MonoBehaviour
{
    //public static SSH_PlayerMove Instance;
    private void Awake()
    {
        //Instance = this;
    }

    public float walkSpeed = 8;
    public float runSpeed = 15;
    public float sprintSpeed = 30;
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
    public float jumpRayLen = 2f;

    float speed;
    float maxWebSwingSpeed = 50;
    float webSwingingTime = 0;
    float webSwingStartSpeed = 0;
    Vector3 webSwingEndVelocity;
    Vector3 inertiaVelocity;
    public float webJumpFactor = 3f;
    bool startFlag = false;


    RaycastHit wallHit;
    RaycastHit wallHitR;
    RaycastHit wallHitL;
    bool isWall = false;
    bool wallFlag = true;

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
        Sticking,
        WallRunVertical,
        WallRunHorizontal,
    }
    public MoveState moveState = MoveState.Normal;
    #endregion

    public void ChangeState(MoveState s)
    {
        if (moveState == s) return;

        moveState = s;

        switch (moveState)
        {
            case MoveState.Normal:
                break;

            case MoveState.Floating:
                anim.SetTrigger("Floating");
                break;

            case MoveState.WebSwing:
                anim.SetTrigger("WebSwing");
                break;

            case MoveState.WebZip:
                anim.SetTrigger("Flying");
                break;

            case MoveState.PointWebZip:
                break;

            case MoveState.PointLaunch:
                break;

            case MoveState.Sticking:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        KeyClickManager();
        WallCheck();

        anim.SetBool("IsFloating", (moveState == MoveState.Floating));

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

            case MoveState.Sticking:
                Sticking();
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

    float stickRotation;
    Vector3 stickPoint;
    Vector3 stickUpDir;

    private void Sticking()
    {
        // 시작할 때 한번만 들어온다
        if (wallFlag)
        {
            wm.isGoWebSwing = false;
            wm.isGoWebZip = false;
            wm.isGoPointWebZip = false;
            //wm.isGoPointLaunch = false;

            wm.isWebSwingSuccess = false;
            wm.isWebZipsuccess = false;
            wm.isPointWebZipsuccess = false;

            stickPoint = new Vector3(transform.position.x, wallHit.point.y, transform.position.z);
            stickUpDir = wallHit.normal;

            yVelocity = 0;
            wallFlag = false;
        }

        PlayerRotate(MoveState.Sticking);
        InputManage(MoveState.WebSwing);
        Jump(MoveState.Sticking);

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 75, Time.deltaTime * FOVChangeSpeed);

        if (IsJumping())
        {
            //moveState = MoveState.Normal;
            ChangeState(MoveState.Normal);
            anim.SetTrigger("Idle");
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            Vector3 dir = body.forward + body.up * 0.5f;
            dir.Normalize();
            webSwingEndVelocity = dir * 200;
            print(1);
        }
    }

    float currentTime;
    public float offsetTime = 1;
    bool isWebJump = false;

    /******************************** State Method ***********************************/
    private void Normal()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime * FOVComebackSpeed);
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump(MoveState.Normal);

        // 점프하면 Floating 상태로 전환하고 싶다. 
        if (IsJumping())
        {
            ChangeState(MoveState.Floating);
        }

        if (isClickMouse2)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                wm.isGoPointWebZip = true;
                wm.pointWebZipFlag = true;
                wm.isFinishSkill = false;
                ChangeState(MoveState.PointWebZip);
            }
        }
    }

    private void Floating()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialFOV, Time.deltaTime * FOVComebackSpeed);
        PlayerRotate(MoveState.Normal);
        InputManage(MoveState.Normal);
        Jump(MoveState.Floating);
        wm.isFinishSkill = true;

        if (!IsJumping())
        {
            // moveState = MoveState.Normal;
            ChangeState(MoveState.Normal);
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
                //moveState = MoveState.PointWebZip;
                ChangeState(MoveState.PointWebZip);
            }
        }

        // Web Swing 시작
        if (Input.GetKeyDown(KeyCode.E))
        {
            wm.isGoWebSwing = true;
            wm.webSwingFlag = true;
            //moveState = MoveState.WebSwing;
            ChangeState(MoveState.WebSwing);
            //transform.forward = body.forward;
            //body.forward = transform.forward;
        }

        // Web Zip 시작
        if (Input.GetButtonDown("Jump"))
        {
            wm.isGoWebZip = true;
            wm.webZipFlag = true;
            //moveState = MoveState.WebZip;
            ChangeState(MoveState.WebZip);
            //webSwingEndVelocity = (transform.forward + Vector3.up * 0.5f) * webZipFactor;
        }
    }

    private void WebSwing()
    {
        if (wm.isWebSwingSuccess == false)
        {
            wm.isGoWebSwing = false;
            wm.isWebSwingSuccess = false;
            ChangeState(MoveState.Floating);
            return;
        }

        PlayerRotate(MoveState.WebSwing);
        InputManage(MoveState.WebSwing);
        Jump(MoveState.WebSwing);
        dir = body.forward;

        if (Input.GetButtonDown("Jump"))
        {
            wm.isGoWebSwing = false;
            wm.isWebSwingSuccess = false;
            ChangeState(MoveState.Floating);
            webSwingEndVelocity = rb.velocity * webJumpFactor;
            isWebJump = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            yVelocity = 0;
            wm.isGoWebSwing = false;
            wm.isWebSwingSuccess = false;
            ChangeState(MoveState.Floating);
            webSwingEndVelocity = rb.velocity;
            print(rb.velocity);
        }
    }

    public float webZipSpeed = 130;

    private void WebZip()
    {
        PlayerRotate(MoveState.Normal);
        Jump(MoveState.WebZip);
        // Web Zip 시작
        currentTime += Time.deltaTime;

        if (wm.isWebZipsuccess)
        {
            Vector3 webZipDir = body.forward + body.up * 0.4f;
            webZipDir.Normalize();

            webZipSpeed = Mathf.Lerp(webZipSpeed, 20, Time.deltaTime * 2.5f);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 80, Time.deltaTime * FOVChangeSpeed);

            rb.velocity = webZipDir * webZipSpeed;
        }

        //print(webZipDir + ", " + rb.velocity + ", " + rb.velocity.magnitude);
        // Web Zip 끝
        if (currentTime > 0.5f)
        {
            wm.isGoWebZip = false;
            wm.isWebZipsuccess = false;
            wm.isFinishSkill = true;
            ChangeState(MoveState.Floating);
            webSwingEndVelocity = rb.velocity;

            currentTime = 0;
        }
        // -> 현재 문제 rb.velocity값이 왔다갔다함. (정상값, (0, 1, 0)의 스케일값, 크기가 작은 값)
    }

    public Vector3 reachPoint;
    float initialFOV = 60;
    float FOVChangeSpeed = 5;
    float FOVComebackSpeed = 2;
    float reachTime = 0;

    private void PointWebZip()
    {
        Jump(MoveState.PointWebZip);
        currentTime += Time.deltaTime;

        if (wm.isPointWebZipsuccess)
        {
            transform.position = Vector3.Lerp(transform.position, reachPoint, Time.deltaTime * 5);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 80, Time.deltaTime * FOVChangeSpeed);
        }
        else
        {
            wm.isGoPointWebZip = false;
            wm.isFinishSkill = true;
            ChangeState(MoveState.Floating);

            currentTime = 0;
        }

        //print("currentTime : " + currentTime);
        if (Vector3.Distance(transform.position, reachPoint) < 1.5)
        {
            transform.position = reachPoint - Vector3.up * 0.8f;
            reachTime += Time.deltaTime;
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (isClickJump)
            {
                //moveState = MoveState.PointLaunch;
                ChangeState(MoveState.PointLaunch);
            }

            if (Physics.Raycast(ray, out hit, 2f) || reachTime > 1)
            {
                wm.isGoPointWebZip = false;
                wm.isPointWebZipsuccess = false;
                wm.isFinishSkill = true;
                //moveState = MoveState.Normal;
                ChangeState(MoveState.Normal);

                currentTime = 0;
            }
        }
    }

    float pointLaunchDelayTime = 0;
    float pointLaunchPower = 50;
    Vector3 pointLaunchDir;

    private void PointLaunch()
    {
        pointLaunchDelayTime += Time.deltaTime;
        Jump(MoveState.PointWebZip);

        pointLaunchDir = body.forward * 1 + body.up * 0.3f;
        pointLaunchDir.Normalize();

        rb.AddForce(pointLaunchDir * pointLaunchPower, ForceMode.Impulse);

        if (pointLaunchDelayTime > 0.3f)
        {
            wm.isGoPointWebZip = false;
            wm.isPointWebZipsuccess = false;
            wm.isFinishSkill = true;
            webSwingEndVelocity = rb.velocity;
            //rb.AddForce(pointLaunchDir * pointLaunchPower, ForceMode.Impulse);
            ChangeState(MoveState.Floating);
            pointLaunchDelayTime = 0;
        }


        //Ray ray = new Ray(transform.position, -transform.up);
        //RaycastHit hit;

            //// 도착하면
            //if (Vector3.Distance(transform.position, reachPoint) < 2)
            //{
            //    if (Physics.Raycast(ray, out hit, 3f))
            //    {
            //        Vector3 launchDir = wm.offsetDir + Vector3.up * 0.5f;

            //        //webSwingEndVelocity = launchDir * 5;
            //        //wm.isGoPointWebZip = false;
            //        //wm.isFinishSkill = true;
            //        //moveState = MoveState.Floating;

            //        currentTime = 0;
            //    }
            //}
    }

    private void WallRunVertical()
    {
        if (!isWall)
        {
            moveState = MoveState.Normal;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            ChangeState(MoveState.Floating);
        }
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
            //walkSpeed = Mathf.Lerp(walkSpeed, sprintSpeed, Time.deltaTime);
            speed = walkSpeed;
            webSwingEndVelocity /= 1.5f; // 마찰력 + 정지하려는 힘 -> 관성 확 줄이기
        }
        else if (moveState == MoveState.Sticking)
        {
            speed = 30;
            webSwingEndVelocity /= 1.5f;
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
                webSwingStartSpeed = Math.Clamp(rb.velocity.magnitude, walkSpeed + 2, 30);
                startFlag = false;
            }
            webSwingingTime += Time.deltaTime;
            speed = Math.Clamp(webSwingStartSpeed * (2 + webSwingingTime * 1.5f), 30, maxWebSwingSpeed);

            //print("speed : " + speed);
        }

        // 웹스윙이 끝났을때의 속도를 받고 싶다.
        if (webSwingEndVelocity.magnitude < 0.1f)
        {
            webSwingEndVelocity = Vector3.zero;
            isWebJump = false;
        }
        else if (isWebJump)
        {
            webSwingEndVelocity = Vector3.Lerp(webSwingEndVelocity, Vector3.zero, Time.deltaTime * 1.2f);
        }
        else
        {
            webSwingEndVelocity = Vector3.Lerp(webSwingEndVelocity, Vector3.zero, Time.deltaTime * 2.5f);
        }

        inertiaVelocity = webSwingEndVelocity;

        

        //print(inertiaVelocity);
        if (moveState != MoveState.WebZip)
        {
            rb.velocity = dir * speed + inertiaVelocity;
        }

        anim.SetFloat("Speed", rb.velocity.magnitude);
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
        else if (state == MoveState.Sticking)
        {
            if (Vector3.Angle(transform.up, stickUpDir) < 20)
            {
                transform.up = stickUpDir;
            }
            else
            {
                transform.up = Vector3.Lerp(transform.up, stickUpDir, Time.deltaTime * 10);
                transform.position = Vector3.Lerp(transform.position, stickPoint, Time.deltaTime);
            }

            float dot = Vector3.Dot(camPivot.right, stickUpDir);


            body.localEulerAngles = new Vector3(0, cpr.yRot, 0);

            //if (cpr.yRot)

            //if (dot >= 0.65f)
            //    yRotation = 90;
            //else if (dot <= -0.65f)
            //    yRotation = -90f;
            //else
            //    yRotation = 0;

            //rotY = Mathf.Lerp(rotY, yRotation, Time.deltaTime * 5);
            ////body.localEulerAngles = new Vector3(0, yRotation, 0);
            //Vector3 dir = new Vector3(0, rotY, 0);
            //body.localEulerAngles = dir;

            // Vector3.Lerp(body.localEulerAngles, dir, Time.deltaTime * 5);


            //if (dot >= 0.65f)
            //    dir = Vector3.Cross(stickUpDir, Vector3.up).normalized;
            //else if (dot <= -0.65f)
            //    dir = Vector3.Cross(stickUpDir, Vector3.down).normalized;
            //else
            //    dir = Vector3.up;

            //print("dot : " + dot + "\t dir : " + dir);


            //if (MathF.Abs(dot) >= 0.8f)
            //{
            //    body.forward = dir;
            //}
            //else
            //    body.forward = Vector3.Lerp(body.forward, dir, Time.deltaTime * 5);

            //if (MathF.Abs(dot) >= 0.8f)
            //{
            //    body.localEulerAngles = dir;
            //}
            //else
            //    body.localEulerAngles = Vector3.Lerp(body.localEulerAngles, dir, Time.deltaTime * 5);

            //Vector3 dir = Quaternion.AngleAxis(yRotation, transform.up) * body.forward;
        }
    }

    void InputManage(MoveState state)
    {
        if (state == MoveState.Normal)
        {
            float v = Input.GetAxisRaw("Vertical");
            float h = Input.GetAxisRaw("Horizontal");

            //if (v == 0)
            //{
            //    anim.SetTrigger("Idle");
            //}
            //else
            //{
            //    anim.SetTrigger("Walk");
            //}

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
        else if (state == MoveState.WebSwing || state == MoveState.WebZip
            || state == MoveState.PointWebZip)
        {
            yVelocity = 0;
        }
        else if (state == MoveState.Sticking)
        {
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
        }

        if (state == MoveState.Sticking) return;

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
    float jumpButtonTime = 0;

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
            jumpButtonTime += Time.deltaTime;
            if (jumpButtonTime > 0.5f)
            {
                isClickJump = false;
                jumpButtonTime = 0;
            }
        }
    }

    public float wallCheckRayLen = 2;
    public int wallCeckCount = 10;
    Vector3 wallCheckDir;

    bool WallCheckFromRight(out RaycastHit hit)
    {
        Ray wallCheckRay;

        Vector3 initalDir = Quaternion.AngleAxis(-35, body.up) * body.right;

        for (float i = 0; i <= 55; i += (55 / wallCeckCount))
        {
            wallCheckDir = Quaternion.AngleAxis(-i, body.up) * initalDir;
            wallCheckRay = new Ray(body.position, wallCheckDir);

            Debug.DrawRay(body.position, wallCheckDir * (wallCheckRayLen - (1 - i / 55)) , Color.white);

            if (Physics.Raycast(wallCheckRay, out hit, wallCheckRayLen, LayerMask.GetMask("Wall")))
            {
                Debug.DrawRay(wallCheckRay.origin, wallCheckRay.direction * (wallCheckRayLen - (1 - i / 55)), Color.red);
                return true;
            }
        }
        hit = new RaycastHit();
        return false;
    }
    bool WallCheckFromLeft(out RaycastHit hit)
    {
        Ray wallCheckRay;

        Vector3 initalDir = Quaternion.AngleAxis(35, body.up) * -body.right;

        for (float i = 0; i <= 55; i += (55 / wallCeckCount))
        {
            wallCheckDir = Quaternion.AngleAxis(i, body.up) * initalDir;
            wallCheckRay = new Ray(body.position, wallCheckDir);

            Debug.DrawRay(body.position, wallCheckDir * (wallCheckRayLen - (1 - i / 55)), Color.gray);

            if (Physics.Raycast(wallCheckRay, out hit, wallCheckRayLen, LayerMask.GetMask("Wall")))
            {
                Debug.DrawRay(wallCheckRay.origin, wallCheckRay.direction * (wallCheckRayLen - (1 - i / 55)), Color.yellow);
                return true;
            }
        }
        hit = new RaycastHit();
        return false;
    }

    float rotY;
    void WallCheck()
    {
        if (WallCheckFromLeft(out wallHitL))
            isWall = true;
        else
            isWall = false;

        if (WallCheckFromRight(out wallHitR))
            isWall = true;

        if (isWall)
        {
            if (wallHitL.distance > wallHitR.distance)
                wallHit = wallHitR;
            else
                wallHit = wallHitL;

            print(wallHit.distance);
            Debug.DrawLine(wallHit.point, wallHit.point + wallHit.normal, Color.green);
            if ((wallHit.distance < 1.2f))
            {
                moveState = MoveState.Sticking;
                wallFlag = true;
                rotY = body.localEulerAngles.y;
            }
        }
    }
}
