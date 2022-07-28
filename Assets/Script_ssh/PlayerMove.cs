using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력을 받아 이동하고 싶다.
public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 10;
    float speed = 0;
    float time = 0;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 dir = Vector3.forward * v + Vector3.right * h;
        dir.Normalize();

        
        if (v != 0 || h != 0)
        {
            time += Time.deltaTime * 10;
            speed = Mathf.Lerp(5, walkSpeed, time);
        }
        else
        {
            time = 0;
        }

        print(speed + "     " + time);
        rb.velocity = dir * speed;
    }
}
