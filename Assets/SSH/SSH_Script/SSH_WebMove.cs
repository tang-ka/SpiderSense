using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_WebMove : MonoBehaviour
{
    //LineRenderer[] line = new LineRenderer[2];
    LineRenderer rightLine;
    LineRenderer leftLine;
    public Transform rightHand;
    public Transform leftHand;
    public Transform[] hook;
    public Transform body;


    public Vector3 webDir;

    public bool isGoWebSwing = false;
    public bool isGoWebZip = false;
    public bool isGoPointWebZip = false;
    public bool isGoPointLaunch = false;

    float currentTime;
    public float keyDelayTime = 0.2f;
    public float deadLineTime = 0.5f;


    public bool webSwingFlag = false;
    public bool webZipFlag = false;
    public bool pointWebZipFlag = false;

    public bool isWebSwingSuccess = false;
    public bool isWebZipsuccess = false;
    public bool isPointWebZipsuccess = false;

    public Vector3 offsetDir;

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
    RaycastHit webSwingPoint = new RaycastHit();

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitTest;
        //bool test = RayForDetection(out hitTest);
        //if (EdgeDetection(out hitTest))
        WebSwingDetection(out hitTest);

        KeyClickManager();

        // 웹스윙
        if (isGoWebSwing)
        {
            isFinishSkill = false;

            RaycastHit hit;
            
            if (WebSwingDetection(out hit))
            {
                isWebSwingSuccess = true;

                if (webSwingFlag)
                {
                    webSwingPoint = hit;
                    webSwingFlag = false;
                }
            }
            if (isWebSwingSuccess)
                ShootWeb(rightHand.position, webSwingPoint.point, out webDir);
        }
        // 웹 집
        else if (isGoWebZip)
        {
            isFinishSkill = false;
            RaycastHit hit;
            if (RayForDetection(out hit) && webZipFlag)
            {
                ShootWeb(rightHand.position, hit.point, out webDir);

                isWebZipsuccess = true;
                webZipFlag = false;
            }
        }
        // 포인트 웹 집
        else if (isGoPointWebZip)
        {
            RaycastHit hit;
            if (EdgeDetection(out hit) && pointWebZipFlag)
            {
                offsetDir = hit.point - transform.position;
                offsetDir.Normalize();

                Vector3 offset = offsetDir * 0.6f + Vector3.up * 0.8f;
                SSH_PlayerMove.Instance.reachPoint = hit.point + offset;

                ShootWeb(rightHand.position, hit.point, out webDir);
                ShootWeb(leftHand.position, hit.point, out webDir);

                isPointWebZipsuccess = true;
                pointWebZipFlag = false;
            }
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

    public void ShootWeb(Vector3 start, Vector3 end, out Vector3 webDir)
    {
        LineRenderer line;
        webDir = end - start;
        float webLen = Vector3.Distance(start, end);
        RaycastHit hit;

        if (start == rightHand.position)
            line = rightLine;
        else
            line = leftLine;

        Ray web = new Ray(start, webDir);
        //Debug.DrawRay(web.origin, web.direction * webLen, Color.white);
        line.SetPosition(0, start);

        if (Physics.Raycast(web, out hit, webLen * 2))
        {
            //Debug.DrawRay(transform.position, transform.up * webLen, Color.red);
            line.SetPosition(1, hit.point);
        }

        line.enabled = true;
    }

    int rayCount = 50;
    int detectionRange = 80;
    int detectionHalfAngle = 60;

    #region RayForDetection()
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
            //Debug.DrawLine(rayR.origin, hitInfoR.point, Color.cyan);
            dirR = dirL = hitInfoR.point - rayR.origin;
            hitInfoL = hitInfoR;
        }

        hit = hitInfoR;
        return false;
    }
    #endregion

    float ED_rayCount = 300;
    float ED_detectionRange = 500;
    float ED_detectionStep = 0.1f;

    float reachableMaxDistance = 120;
    float reachableMinDistance = 20;
    float depthOffset = 3;
    float heightOffset = 5;
    Vector3 detectionPoint;
    Vector3 sightPoint;

    #region EdgeDetection()
    public bool EdgeDetection(out RaycastHit hit)
    {
        Ray ray;
        RaycastHit curHit;
        RaycastHit preHit;
        RaycastHit preEvenHit;
        RaycastHit preOddHit;
        RaycastHit hitNull;

        Vector3 dir;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out curHit, reachableMaxDistance))
        {
            if (curHit.distance < reachableMinDistance) { goto Finish; }
            //Debug.DrawLine(ray.origin, curHit.point, Color.red);

            if (curHit.distance > 50)
                heightOffset = 8;
            else
                heightOffset = 5;

            // 수직벽
            if (Vector3.Dot(curHit.normal, Vector3.up) < 0.03f)
            {
                sightPoint = curHit.point;
                detectionPoint = curHit.point - (curHit.normal * depthOffset) + (Vector3.up * heightOffset);
                //Debug.DrawLine(detectionPoint, detectionPoint - (Vector3.up * heightOffset), Color.green);

                ray = new Ray(detectionPoint, Vector3.down);
                if (Physics.Raycast(ray, out curHit, ED_detectionRange))
                {
                    preHit = curHit;

                    for (int i = 1; i < ED_rayCount / 3; i++)
                    {
                        dir = sightPoint - detectionPoint;
                        dir.y = 0;
                        dir.Normalize();

                        ray = new Ray(detectionPoint + (dir * ED_detectionStep * i), Vector3.down);

                        if (Physics.Raycast(ray, out curHit, ED_detectionRange))
                        {
                            if (curHit.distance - preHit.distance > 3)
                            {
                                Debug.DrawLine(ray.origin, preHit.point, Color.blue);
                                hit = preHit;
                                return true;
                            }

                            preHit = curHit;
                        }
                    }
                }
            }
            // 평면벽
            else if (Vector3.Dot(curHit.normal, Vector3.up) >= 0.85f)
            {
                detectionPoint = curHit.point + (Vector3.up * heightOffset);
                Debug.DrawLine(detectionPoint, detectionPoint - (Vector3.up * heightOffset), Color.green);

                ray = new Ray(detectionPoint, Vector3.down);
                if (Physics.Raycast(ray, out curHit, ED_detectionRange))
                {
                    preEvenHit = preOddHit = curHit;

                    for (int i = 1; i < ED_rayCount; i++)
                    {
                        dir = Camera.main.transform.position - detectionPoint;
                        dir.y = 0;
                        dir.Normalize();

                        if (i % 2 == 0)
                            ray = new Ray(detectionPoint + (dir * ED_detectionStep * i), Vector3.down);
                        else
                            ray = new Ray(detectionPoint + (-dir * ED_detectionStep * i), Vector3.down);

                        if (Physics.Raycast(ray, out curHit, ED_detectionRange))
                        {
                            if (i % 2 == 0 && curHit.distance - preEvenHit.distance > 3)
                            {
                                Debug.DrawLine(ray.origin, preEvenHit.point, Color.blue);
                                hit = preEvenHit;
                                return true;
                            }
                            else if (i % 2 != 0 && curHit.distance - preOddHit.distance > 3)
                            {
                                Debug.DrawLine(ray.origin, preOddHit.point, Color.blue);
                                hit = preOddHit;
                                return true;
                            }

                            if (i % 2 == 0)
                                preEvenHit = curHit;
                            else
                                preOddHit = curHit;
                        }
                    }
                }
            }
        }
        Finish:

        ray = new Ray(Camera.main.transform.position, -Camera.main.transform.up);

        if (Physics.Raycast(ray, out hitNull, detectionRange))
        {
            //Debug.DrawLine(ray.origin, hitNull.point, Color.black);
        }
        hit = hitNull;
        return false;
    }
    #endregion

    int countC = 20;
    int firstTry = 15;
    int secondTry = 40;
    int thirdTry = 60;

    int maxLen = 80;
    int startAngle = 25;

    bool WebSwingDetection(out RaycastHit hit)
    {
        Ray rayR;
        Ray rayC;
        Ray rayL;
        RaycastHit hitInfoR;
        RaycastHit hitInfoL;
        Vector3 dirR = Vector3.zero;
        Vector3[] dirC = new Vector3[countC + 1];
        Vector3 dirL = Vector3.zero;

        // Center Ray를 쏘고 싶다.
        // 방향
        //dirC = Quaternion.AngleAxis(-70, body.right) * body.forward;
        //dirC.Normalize();
        //rayC = new Ray(body.position, dirC);
        //Debug.DrawLine(body.position, body.position + dirC * 50, Color.black);

        for (int i = 0; i <= countC; i++)
        {
            dirC[i] = Quaternion.AngleAxis((startAngle - 90) - i, body.right) * body.forward;
            dirC[i].Normalize();
            rayC = new Ray(body.position, dirC[i]);
            Debug.DrawLine(body.position, body.position + dirC[i] * maxLen, Color.white);

            for (int j = 1; j <= firstTry; j++)
            {
                dirR = Quaternion.AngleAxis(-1 * j / 2, body.forward) * dirC[i];
                dirR.Normalize();
                rayR = new Ray(body.position, dirR);

                dirL = Quaternion.AngleAxis(1 * j / 2, body.forward) * dirC[i];
                dirL.Normalize();
                rayL = new Ray(body.position, dirL);

                if (Physics.Raycast(rayR, out hitInfoR, maxLen - j / 4))
                {
                    Debug.DrawLine(body.position, body.position + dirR * (maxLen + j), Color.blue);
                    hit = hitInfoR;
                    return true;
                }
                else if (Physics.Raycast(rayL, out hitInfoL, maxLen - j / 4))
                {
                    Debug.DrawLine(body.position, body.position + dirL * (maxLen + j), Color.blue);
                    hit = hitInfoL;

                    return true;
                }
            }
        }

        for (int i = 0; i <= countC; i++)
        {
            for (int k = firstTry + 1; k <= secondTry; k++)
            {
                dirR = Quaternion.AngleAxis(-1 * k / 2, body.forward) * dirC[i];
                dirR.Normalize();
                rayR = new Ray(body.position, dirR);

                dirL = Quaternion.AngleAxis(1 * k / 2, body.forward) * dirC[i];
                dirL.Normalize();
                rayL = new Ray(body.position, dirL);

                if (Physics.Raycast(rayR, out hitInfoR, maxLen - k / 3))
                {
                    Debug.DrawLine(body.position, body.position + dirR * (maxLen + k), Color.green);
                    hit = hitInfoR;

                    return true;
                }
                else if (Physics.Raycast(rayL, out hitInfoL, maxLen - k / 3))
                {
                    Debug.DrawLine(body.position, body.position + dirL * (maxLen + k), Color.green);
                    hit = hitInfoL;

                    return true;
                }
            }
        }

        //for (int i = 0; i <= countC; i++)
        //{
        //    for (int l = secondTry + 1; l <= thirdTry; l++)
        //    {
        //        dirR = Quaternion.AngleAxis(-1 * l / 2, body.forward) * dirC[i];
        //        dirR.Normalize();
        //        rayR = new Ray(body.position, dirR);

        //        dirL = Quaternion.AngleAxis(1 * l / 2, body.forward) * dirC[i];
        //        dirL.Normalize();
        //        rayL = new Ray(body.position, dirL);

        //        if (Physics.Raycast(rayR, out hitInfoR, maxLen - l / 2.5f))
        //        {
        //            Debug.DrawLine(body.position, body.position + dirR * (maxLen - l / 2.5f), Color.yellow);
        //            hit = hitInfoR;

        //            return true;
        //        }
        //        else if (Physics.Raycast(rayL, out hitInfoL, maxLen - l / 2.5f))
        //        {
        //            Debug.DrawLine(body.position, body.position + dirL * (maxLen - l / 2.5f), Color.yellow);
        //            hit = hitInfoL;

        //            return true;
        //        }
        //    }
        //}

        rayR = new Ray(Camera.main.transform.position, -Camera.main.transform.up);

        if (Physics.Raycast(rayR, out hitInfoR, detectionRange))
        {
            //Debug.DrawLine(rayR.origin, hitInfoR.point, Color.cyan);
            dirR = dirL = hitInfoR.point - rayR.origin;
            hitInfoL = hitInfoR;
        }

        hit = hitInfoR;
        return false;
    }
}
