using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//agent기능을 이용해서 목적지를 향해서 이동하고 싶다.
// 목표검색, 이동, 공격

public class BeforeEnemy : MonoBehaviour
{
    enum State { SEARCH, MOVE, ATTACK }

    //현재 상태
    State enemyState;

    NavMeshAgent agent;
    GameObject target;
    Animator anim;


    void Start()
    {
        enemyState = State.SEARCH; //초기 상태는 적을 검색하는 행위
        agent = this.GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        switch (enemyState)
        {
            //현재 상태가 SEARCH라면
            case State.SEARCH: UpdateSearch(); break;
            //상태가 이동이라면 이동만 하고싶다
            case State.MOVE: UpdateMove(); break;
            //상태가 공격이라면 공격만 하고싶다.
            case State.ATTACK: UpdateAttack(); break;

        }
    }

    private void UpdateAttack()
    {
        //만약 목적지와의 거리가 공격가능거리가 아니라면
        //다시 이동상태로 전이하고싶다

    }

    private void UpdateMove()
    {
        //agent에게 목적지를 알려주고 싶다.
        agent.destination = target.transform.position;
        //만약 목적지와의 거리가 <=  공격가능거리라면?
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= agent.stoppingDistance)
        {
            //공격상태로 전이하고싶다.
            enemyState = State.ATTACK;
            anim.SetTrigger("Attack");
        }
    }

    private void UpdateSearch()
    {
        //목적지를 찾고싶다
        target = GameObject.Find("Player");
        //만약 목적지를 찾았으면
        if (target != null)
        {
            //이동상태로 전이하고싶다.
            enemyState = State.MOVE;
            //외부 animator.trigger로 설정한 것 중 무브 실행
            anim.SetTrigger("Move");
        }
    }
    public void OnEnemyAttackHit()
    {
        print("OnEnemyAttackHit");
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= agent.stoppingDistance)
        {


            print("hit!!!");
        }
        //만약 공격가능거리라면 Hit를 하고싶다.

    }
    public void OnEnemyAttackFinished()
    {
        print("OnEnemyAttackFinished");
        //만약 공격가능거리가 아니라면
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > agent.stoppingDistance)
        {
            //이동상태로 전이하고 싶다.
            enemyState = State.MOVE;
            anim.SetTrigger("Move");

        }

    }
}
