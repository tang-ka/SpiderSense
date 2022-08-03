using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KN_Enemy : MonoBehaviour
{
   public enum State
    {
        Idle,
        Move,
        Attack1,
        Attack2,

    }

    public State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        target = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Idle)
        {
            UpdateIdle();
        }
        else if (state == State.Move)
        {
            UpdateMove();
        }
        else if (state == State.Attack1)
        {
            UpdateAttack1();
        }
        else if (state == State.Attack2)
        {
            UpdateAttack2();
        }

    }



    GameObject target;
    public float findDistance = 5;
    private void UpdateIdle()
    {
        //target이 감지거리안에 들어오면 Move로 전이
        //1.나와 target의 거리구함
        float distance = Vector3.Distance(transform.position, target.transform.position);
        //2.만약 그 거리가 감지거리보다 작다면
        if(distance < findDistance)
        {
            //3. move상태로 전이
            state = State.Move;
        }
    }

    public float speed = 1;
    public float attackDistance = 1.5f;
    private void UpdateMove()
    {
        // target방향으로 이동하다가 target이 공격거리 안에 들어온다면 attack으로 전이
        // 1.target방향으로 이동하고싶다.
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
        // 2.나와 target의 거리구함
        float distance = Vector3.Distance(transform.position, target.transform.position);
        // 3.그 거리가 공격거리보다 작다면 
        if (distance < attackDistance)
        {
        // 4.attack상태 전이
            state = State.Attack1;
        }
    }

    float currentTime;
    float attackTime = 1;
    private void UpdateAttack1()
    {
        //일정시간마다 공격하되 공격시점에 target이 공격거리 밖에 있다면 move로, 아니라면 계속 공격
        //1. 시간이흐르다가
        currentTime += Time.deltaTime;
        //2. 만약 현재시간이 공격시간이 되면
        if(currentTime > attackTime)
        {
            //3. 현재시간을 초기화하고
            currentTime = 0;
            //4. 플레이어를 공격하고
            //target.AddDamage();
            //5.1 나와 target의 거리구함
            float distance = Vector3.Distance(transform.position, target.transform.position);
            //5.2 그 거리가 공격거리보다 크다면
            if(distance > attackDistance)
            {
                //5.3 Move상태 전이
                state = State.Move;
            }
        }

    }

    private void UpdateAttack2()
    {
        currentTime += Time.deltaTime;



    }



    //player에 AddDamage 추가하기
    //public void AddDamage(int damage)
    //{
    //    Destroy(gameObject);
    //}
}
