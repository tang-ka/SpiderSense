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

    [HideInInspector]
    public bool isClickE;
    bool isClickLShift;
    // Update is called once per frame
    void Update()
    {
        KeyClickManager();

        if (isClickE)
        {
            currentTime += Time.deltaTime;

            if (isGoWebSwing)
            {
                RaycastHit hit;
                ShootWeb(rightHand.position, hook[hookCount].position, out webDir, out hit);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("Web Jump");
            }
        }
        else if (!isClickE)
        {
            rightLine.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            hookCount++;
        }

        if (isGoWebZip)
        {
            RaycastHit hitL, hitR;
            ShootWeb(rightHand.position, hook[hookCount].position, out webDir, out hitR);
            ShootWeb(leftHand.position, hook[hookCount].position, out webDir, out hitL);
        }
        else if (!isGoWebZip)
        {
            rightLine.enabled = false;
            leftLine.enabled = false;
        }

        //if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.E)) || (Input.GetKey(KeyCode.E) && Input.GetKeyDown(KeyCode.LeftShift)))
        //{
        //    print("Point Web Zip");
        //}
    }

    void KeyClickManager()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isClickE = true;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            isClickE = false;
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
}
