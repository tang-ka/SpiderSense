using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_CamOffset : MonoBehaviour
{
    Transform mainCam;
    Vector3 initialLocalPosition;
    Vector3 offsetLocalPosition;

    Vector3 curVelocity;
    Vector3 preVelocity;

    public GameObject player;
    Rigidbody body;

    float yOffset;
    float xOffset;
    float zOffset;

    public float yScale = 0.1f;
    public float xScale = 0.1f;
    public float zScale = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        body = player.GetComponent<Rigidbody>();
        initialLocalPosition = mainCam.localPosition;
        preVelocity = player.transform.position;
    }

    int count = 0;
    // Update is called once per frame
    void Update()
    {
        curVelocity = body.velocity;
        if (count > 5)
        {
            preVelocity = body.velocity;
            count = 0;
        }

        Vector3 acceleration = curVelocity - preVelocity;
        count++;


        print(acceleration);

        yOffset = -body.velocity.y;
        xOffset = -body.velocity.x;
        zOffset = -body.velocity.z;

        //print(yOffset);

        if (Mathf.Abs(yOffset) < 40)
            offsetLocalPosition.y = Mathf.Lerp(offsetLocalPosition.y, initialLocalPosition.y, Time.deltaTime * 5);
        else if (yOffset > 40)
            offsetLocalPosition.y = Mathf.Lerp(offsetLocalPosition.y, initialLocalPosition.y + 0.05f * yOffset, Time.deltaTime * 5);
        else if (yOffset < -40)
            offsetLocalPosition.y = Mathf.Lerp(offsetLocalPosition.y, initialLocalPosition.y + (yScale * yOffset), Time.deltaTime * 5);

        if (Mathf.Abs(xOffset) < 30)
            offsetLocalPosition.x = Mathf.Lerp(offsetLocalPosition.x, initialLocalPosition.x, Time.deltaTime * 5);
        else if (Mathf.Abs(xOffset) > 30)
            offsetLocalPosition.x = Mathf.Lerp(offsetLocalPosition.x, initialLocalPosition.x + (xScale * xOffset), Time.deltaTime * 5);


        if (Mathf.Abs(zOffset) < 20)
            offsetLocalPosition.z = Mathf.Lerp(offsetLocalPosition.z, initialLocalPosition.z, Time.deltaTime * 5);
        else if (Mathf.Abs(zOffset) > 30)
            offsetLocalPosition.z = Mathf.Lerp(offsetLocalPosition.z, initialLocalPosition.z + (zScale * zOffset), Time.deltaTime * 5);

        //offsetLocalPosition.x = Mathf.Lerp(offsetLocalPosition.x, initialLocalPosition.x + (xScale * xOffset), Time.deltaTime);
        //offsetLocalPosition.z = Mathf.Lerp(offsetLocalPosition.z, initialLocalPosition.z + (zScale * zOffset), Time.deltaTime);

        mainCam.localPosition = offsetLocalPosition;
    }

    private void FixedUpdate()
    {
    }
}
