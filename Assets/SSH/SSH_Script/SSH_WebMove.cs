using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_WebMove : MonoBehaviour
{
    LineRenderer line;
    public Transform rightHand;
    public Transform leftHand;
    public Transform[] hook;

    int hookCount = 0;

    public Vector3 webDir;

    public bool isWebMove = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.startColor = Color.white;
        line.endColor = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (isWebMove == false) 
            {
                RaycastHit hit;
                ShootWeb(rightHand.position, hook[hookCount].position, out webDir, out hit);
            }
            else
            {
                RaycastHit hit;
                ShootWeb(rightHand.position, hook[hookCount].position, out webDir, out hit);
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            line.enabled = false;
            hookCount++;
        }
    }

    public void ShootWeb(Vector3 start, Vector3 end, out Vector3 webDir, out RaycastHit hit)
    {
        webDir = end - start;
        float webLen = Vector3.Distance(start, end);

        Ray web = new Ray(start, webDir);
        //Debug.DrawRay(web.origin, web.direction * webLen, Color.white);
        line.SetPosition(0, start);

        if (Physics.Raycast(web, out hit, webLen * 2))
        {
            Debug.DrawRay(transform.position, transform.up * webLen, Color.red);
            line.SetPosition(1, hit.point);
        }
        print(hit.distance);

        line.enabled = true;
    }
}
