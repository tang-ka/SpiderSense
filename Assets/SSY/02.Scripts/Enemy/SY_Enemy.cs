using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//agent를 이용해서 길찾기를 하고싶다.
//순찰, 추적 상태로 제어하고 싶다.
public class SY_Enemy : MonoBehaviour
{
    enum State
    {
        Patrol,
        Chase,
        Attack
    }
    State state;
    NavMeshAgent agent;
    Animator anim;
    SY_EnemyHP enemyHP;

    //목적지 인덱스 번호
    int targetIndex;

    void Start()
    {
        state = State.Patrol;
        agent = GetComponent<NavMeshAgent>();
        targetIndex = 0;
        anim = GetComponentInChildren<Animator>();
        enemyHP = GetComponent<SY_EnemyHP>();
    }

    void Update()
    {
        switch (state)
        {
            case State.Patrol:
                UpdatePatrol();
                break;
            case State.Chase:
                UpdateChase();
                break;
            case State.Attack:
                UpdateAttack();
                break;
        }

    }
    public float detectedRadius = 5; //감지범위
    GameObject chaseTarget;

    private void UpdatePatrol()
    {
        //주변 반경 5M안에 Player가 있다면
        Collider[] cols = Physics.OverlapSphere(transform.position, detectedRadius);
        for (int i = 0; i < cols.Length; i++) //콜라이더를 모두 배열에 넣다가
        {
            if (cols[i].name.Contains("Player"))//콜라이더를 가지고 있는 것 중 플레이어의 이름을 가진 놈
            {
                //추적 상태로 전이하고 싶다.
                state = State.Chase;
                chaseTarget = cols[i].gameObject; //찾은 것을 지역변수로
                return;
            }
        }

        Vector3 target = SY_PathInfo.instance.wayPoints[targetIndex].transform.position;
        //길을 순환 이동하고 싶다.
        agent.destination = target;
        //만약 도착했다면? ->거리로
        float distance = Vector3.Distance(target, transform.position);
        if (distance <= 2) //1이라는 거리는 상황에 맞게 값을 정해야한다.
        {
            //인덱스를 1증가.
            anim.SetTrigger("Move");
            targetIndex++;
            //만약 targetIndex가 PathInfo.instance.wayPoints배열 크기 이상이라면 0으로 초기화
            if (targetIndex >= SY_PathInfo.instance.wayPoints.Length)
            {
                targetIndex = 0;
            }
        }
    }
    public float chaseDistance = 4;
    public float attackDistance = 2;
    private void UpdateChase()
    {
        agent.destination = chaseTarget.transform.position; //Agent의 목적지를 찾은 플레이어타겟으로!
        //만약 chaseTarget과의 거리가 추적범위를 벗어나면
        float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (distance > chaseDistance)
        {
            //순찰상태로 전이하고싶다.
            state = State.Patrol;
        }
        //만약 목적지와의 거리가 <=  공격가능거리라면?
        float enemyAttackDistance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (enemyAttackDistance <= attackDistance)
        {
            //공격상태로 전이하고싶다.
            state = State.Attack;
            anim.SetTrigger("Attack");
        }

    }
    private void UpdateAttack()
    {

    }
    public void OnEnemyAttackHit()
    {
        print("OnEnemyAttackHit");
        //만약 공격가능거리라면 Hit를 하고싶다.
        float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (distance <= agent.stoppingDistance)
        {
            print("hit!!!"); //적이 스파이더맨을 때린 시점!!
            SY_HitManager.instance.DoHitPlz();
        }

    }
    public void OnEnemyAttackFinished()
    {
        print("OnEnemyAttackFinished");
        //만약 공격가능거리가 아니라면
        float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (distance > agent.stoppingDistance)
        {
            //추적상태로 전이하고 싶다.
            state = State.Chase;
            anim.SetTrigger("Move");

        }

    }

    //Player => Enemy공격함
    // 상혁이형에게 매개변수 넘겨받기.
    public void TryDamage(int damageValue)
    {
        enemyHP.HP -= damageValue;
        if (enemyHP.HP <= 0)
        {
            //죽는 애니메이션이 끝나고
            //Destroy
            Destroy(gameObject);
        }
        else
        {
            //넉백 및 약간의 멈춤,,?

        }

    }


}
