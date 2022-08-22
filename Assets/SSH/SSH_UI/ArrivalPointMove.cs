using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalPointMove : MonoBehaviour
{
    public Transform goal;
    public Transform mainCam;

    Vector3 myPosition;

    float worldSize;
    float width;
    public float offset = 50;

    // Start is called before the first frame update
    void Start()
    {
        worldSize = Camera.main.orthographicSize * 2;
        float meterPerMete = worldSize / Screen.height;
        width = meterPerMete * Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 myPosition;

        // 왼쪽 오른쪽을 구분하고 싶다.

        myPosition = Camera.main.WorldToScreenPoint(goal.position);
        myPosition.z = 0;
        float angleRL = Vector3.Angle(mainCam.forward, goal.position - mainCam.position);
        float angleUD = Vector3.Angle(mainCam.forward, goal.position - mainCam.position);
        angleUD -= 90;

        //print(myPosition + ", " + angleUD);

        if (IsRight(mainCam, goal) && myPosition.x < 0)
        {
            myPosition.x = -myPosition.x + Screen.width;
        }
        else if (IsRight(mainCam, goal) && angleRL > 90 && myPosition.x > 0)
        {
            myPosition.x += Screen.width;
        }
        else if (!IsRight(mainCam, goal) && myPosition.x > Screen.width)
        {
            myPosition.x = -myPosition.x - Screen.width;
        }
        else if (!IsRight(mainCam, goal) && angleRL > 90 && myPosition.x > 0)
        {
            myPosition.x *= -1;
        }

        if (IsUp(mainCam, goal) && myPosition.y < 0)
        {
            myPosition.y = -myPosition.y + Screen.height;
        }
        else if (!IsUp(mainCam, goal) && myPosition.y > Screen.height)
        {
            myPosition.y = -myPosition.y - Screen.height;
        }
        

        myPosition.y = Mathf.Clamp(myPosition.y, 0 + offset, Screen.height - offset);
        myPosition.x = Mathf.Clamp(myPosition.x, 0 + offset, Screen.width - offset);

        transform.position = myPosition;
    }

    bool IsRight(Transform from, Transform to)
    {
        float result = Vector3.Dot(from.right.normalized, (to.position - from.position).normalized);

        // right
        if (result >= 0)
        {
            return true;
        }
        // left
        else /*((result.y - from.y) < 0)*/
        {
            return false;
        }
    }

    bool IsUp(Transform from, Transform to)
    {
        float result = Vector3.Dot(from.up.normalized, (to.position - from.position).normalized);

        // up
        if (result >= 0)
        {
            return true;
        }
        // down
        else /*((result.x - from.x) < 0)*/
        {
            return false;
        }
    }
}
