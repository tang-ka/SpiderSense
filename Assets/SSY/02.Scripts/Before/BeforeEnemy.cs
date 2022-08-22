using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//agent����� �̿��ؼ� �������� ���ؼ� �̵��ϰ� �ʹ�.
// ��ǥ�˻�, �̵�, ����

public class BeforeEnemy : MonoBehaviour
{
    enum State { SEARCH, MOVE, ATTACK }

    //���� ����
    State enemyState;

    NavMeshAgent agent;
    GameObject target;
    Animator anim;


    void Start()
    {
        enemyState = State.SEARCH; //�ʱ� ���´� ���� �˻��ϴ� ����
        agent = this.GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }


    void Update()
    {
        switch (enemyState)
        {
            //���� ���°� SEARCH���
            case State.SEARCH: UpdateSearch(); break;
            //���°� �̵��̶�� �̵��� �ϰ�ʹ�
            case State.MOVE: UpdateMove(); break;
            //���°� �����̶�� ���ݸ� �ϰ�ʹ�.
            case State.ATTACK: UpdateAttack(); break;

        }
    }

    private void UpdateAttack()
    {
        //���� ���������� �Ÿ��� ���ݰ��ɰŸ��� �ƴ϶��
        //�ٽ� �̵����·� �����ϰ�ʹ�

    }

    private void UpdateMove()
    {
        //agent���� �������� �˷��ְ� �ʹ�.
        agent.destination = target.transform.position;
        //���� ���������� �Ÿ��� <=  ���ݰ��ɰŸ����?
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= agent.stoppingDistance)
        {
            //���ݻ��·� �����ϰ�ʹ�.
            enemyState = State.ATTACK;
            anim.SetTrigger("Attack");
        }
    }

    private void UpdateSearch()
    {
        //�������� ã��ʹ�
        target = GameObject.Find("Player");
        //���� �������� ã������
        if (target != null)
        {
            //�̵����·� �����ϰ�ʹ�.
            enemyState = State.MOVE;
            //�ܺ� animator.trigger�� ������ �� �� ���� ����
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
        //���� ���ݰ��ɰŸ���� Hit�� �ϰ�ʹ�.

    }
    public void OnEnemyAttackFinished()
    {
        print("OnEnemyAttackFinished");
        //���� ���ݰ��ɰŸ��� �ƴ϶��
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance > agent.stoppingDistance)
        {
            //�̵����·� �����ϰ� �ʹ�.
            enemyState = State.MOVE;
            anim.SetTrigger("Move");

        }

    }
}
