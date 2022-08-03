using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력을 받아 이동하고 싶다.
public class PlayerMove : MonoBehaviour
{
    public float walkSpeed = 10;
    Vector3 dir = Vector3.zero;

    Rigidbody rb;
    public Transform camDir;

    float gravity = -9.81f;
    public float yVelocity = 0;
    public float jumpPower = 5f;
    public bool isJumping;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Rotate();

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = transform.right * h + transform.forward * v;
        dir.Normalize();

        Jump();

        rb.velocity = dir * walkSpeed;
    }

    // 플레이어가 보는 방향을 카메라의 방향과 맞춰준다.
    void Rotate()
    {
        Vector3 playerForward = camDir.forward;
        playerForward.y = 0;

        transform.forward = playerForward;
    }

    void Jump()
    {
        yVelocity += gravity * Time.deltaTime;
        if (!isJumping)
        {
            yVelocity = 0;
        }
        IsJumping();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        dir.y = yVelocity;
        //print(isJumping);
    }

    void IsJumping()
    {
        Ray ray = new Ray(transform.position + transform.up, -transform.up);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            print("distance : " + hitInfo.distance);
            //print("Info : " + hitInfo.collider.tag);
            
            if (hitInfo.distance < 1.1f && yVelocity <= 0)
            {
                isJumping = false;
                print("땅");
            }
            else
                isJumping = true;
        }
        else
            isJumping = true;
    }
}
