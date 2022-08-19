using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_WebMove : MonoBehaviour
{
    LineRenderer[] line = new LineRenderer[2];
    LineRenderer rightLine;
    LineRenderer leftLine;
    public Transform rightHand;
    public Transform leftHand;
    public Transform[] hook;

    int hookCount = 0;

    public Vector3 webDir;

    public bool isGoWebSwing = false;
    public bool isGoWebZip = false;
    public bool isGoPointWebZip = false;
    public bool isGoPointLaunch = false;

    float currentTime;
    public float keyDelayTime = 0.2f;
    public float deadLineTime = 0.5f;

    public bool webZipFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        rightLine = rightHand.GetComponent<LineRenderer>();
        leftLine = leftHand.GetComponent<LineRenderer>();

        rightLine.positionCount = 2;
        //rightLine.startColor = Color.white;
        //rightLine.endColor = Color.blue;

        leftLine.positionCount = 2;
        //leftLine.startColor = Color.white;
        //leftLine.endColor = Color.blue;
    }

    public bool isClickE;
    public bool isClickLShift;
    public bool isFinishSkill = false;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hitTest;
        //bool test = RayForDetection(out hitTest);
        bool test = EdgeDetection(out hitTest);

        KeyClickManager();

        // 웹스윙
        if (isGoWebSwing)
        {
            isFinishSkill = false;

            RaycastHit hit;
            ShootWeb(rightHand.position, hook[hookCount].position, out webDir, out hit);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hookCount++;
        }
        // 웹 집
        else if (isGoWebZip)
        {
            isFinishSkill = false;
            RaycastHit hit;
            if (RayForDetection(out hit) && webZipFlag)
            {
                ShootWeb(rightHand.position, hit.point, out webDir, out hit);
                webZipFlag = false;
            }
        }
        // 포인트 웹 집
        else if (isGoPointWebZip)
        {
            isFinishSkill = false;
            RaycastHit hitL, hitR;
            ShootWeb(rightHand.position, hook[hookCount].position, out webDir, out hitR);
            ShootWeb(leftHand.position, hook[hookCount].position, out webDir, out hitL);
        }

        if (isFinishSkill)
        {
            rightLine.enabled = false;
            leftLine.enabled = false;
        }
    }

    /**********************************************************************************************/

    void KeyClickManager()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isClickE = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isClickE = false;
            isFinishSkill = true;
            currentTime = 0;
        }
    }

    public void ShootWeb(Vector3 start, Vector3 end, out Vector3 webDir, out RaycastHit hit)
    {
        LineRenderer line;
        webDir = end - start;
        float webLen = Vector3.Distance(start, end);

        if (start == rightHand.position)
            line = rightLine;
        else
            line = leftLine;

        Ray web = new Ray(start, webDir);
        //Debug.DrawRay(web.origin, web.direction * webLen, Color.white);
        line.SetPosition(0, start);

        if (Physics.Raycast(web, out hit, webLen * 2))
        {
            Debug.DrawRay(transform.position, transform.up * webLen, Color.red);
            line.SetPosition(1, hit.point);
        }

        line.enabled = true;
    }

    int rayCount = 50;
    int detectionRange = 80;
    int detectionHalfAngle = 60;

    bool RayForDetection(out RaycastHit hit)
    {
        Ray rayR;
        Ray rayL;
        RaycastHit hitInfoR;
        RaycastHit hitInfoL;
        Vector3 dirR = Vector3.zero;
        Vector3 dirL = Vector3.zero;

        for (int i = 0; i < rayCount; i++)
        {
            //print(i);
            if (i != 0)
            {
                dirR = Quaternion.AngleAxis(detectionHalfAngle / rayCount, Camera.main.transform.up) * dirR;
                dirL = Quaternion.AngleAxis(-detectionHalfAngle / rayCount, Camera.main.transform.up) * dirL;
                rayR = new Ray(Camera.main.transform.position, dirR);
                rayL = new Ray(Camera.main.transform.position, dirL);

                if (Physics.Raycast(rayR, out hitInfoR, detectionRange))
                {
                    Debug.DrawLine(rayR.origin, hitInfoR.point, Color.blue);
                    dirR = hitInfoR.point - rayR.origin;
                    //print("Right : " + i);

                    if (hitInfoR.collider.CompareTag("Wall"))
                    {
                        //print("오른쪽 벽 발견!!");
                        hit = hitInfoR;
                        return true;
                    }
                }
                if (Physics.Raycast(rayL, out hitInfoL, detectionRange))
                {
                    Debug.DrawLine(rayL.origin, hitInfoL.point, Color.blue);
                    dirL = hitInfoL.point - rayL.origin;
                    //print("Left : " + i);

                    if (hitInfoL.collider.CompareTag("Wall"))
                    {
                        //print("왼쪽 벽 발견!!");
                        hit = hitInfoL;
                        return true;
                    }
                }
            }
            else
            {
                rayR = Camera.main.ScreenPointToRay(Input.mousePosition);
                rayL = rayR;

                if (Physics.Raycast(rayR, out hitInfoR, detectionRange))
                {
                    Debug.DrawLine(rayR.origin, hitInfoR.point, Color.blue);
                    dirR = dirL = hitInfoR.point - rayR.origin;
                    hitInfoL = hitInfoR;

                    if (hitInfoR.collider.CompareTag("Wall"))
                    {
                        //print("정면 벽 발견!!");
                        hit = hitInfoR;
                        return true;
                    }
                }
                else
                {
                    dirR = dirL = Camera.main.transform.forward;
                    hitInfoR = hitInfoL = new RaycastHit();
                }
            }
        }

        rayR = new Ray(Camera.main.transform.position, -Camera.main.transform.up);

        if (Physics.Raycast(rayR, out hitInfoR, detectionRange))
        {
            Debug.DrawLine(rayR.origin, hitInfoR.point, Color.cyan);
            dirR = dirL = hitInfoR.point - rayR.origin;
            hitInfoL = hitInfoR;
        }

        hit = hitInfoR;
        return false;
    }

    int EDrayCount = 100;
    int EDdetectionRange = 500;
    int EDdetectionHalfAngle = 30;

    bool EdgeDetection(out RaycastHit hit)
    {
        Ray ray;
        RaycastHit hitInfo;
        Vector3 dir = Vector3.zero;

        for (int i = 0; i < EDrayCount; i++)
        {
            //print(i);
            if (i != 0)
            {
                dir = Quaternion.AngleAxis(EDdetectionHalfAngle / EDrayCount, Camera.main.transform.right) * dir;
                ray = new Ray(Camera.main.transform.position, dir);

                if (Physics.Raycast(ray, out hitInfo, EDdetectionRange))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.blue);
                    dir = hitInfo.point - ray.origin;
                    if (hitInfo.collider.CompareTag("Test"))
                    {
                        //print("오른쪽 벽 발견!!");
                        hit = hitInfo;
                        return true;
                    }
                }
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition + Vector3.up * 10);

                if (Physics.Raycast(ray, out hitInfo, EDdetectionRange))
                {
                    Debug.DrawLine(ray.origin, hitInfo.point, Color.blue);
                    dir = hitInfo.point - ray.origin;

                }
                else
                {
                    dir = Camera.main.transform.forward + Vector3.up * 10;
                    hitInfo = new RaycastHit();
                }
            }
        }

        ray = new Ray(Camera.main.transform.position, -Camera.main.transform.up);

        if (Physics.Raycast(ray, out hitInfo, EDdetectionRange))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.cyan);
            dir = hitInfo.point - ray.origin;
        }

        hit = hitInfo;
        return false;
    }
}
