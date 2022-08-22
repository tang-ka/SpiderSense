using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//agent�� �̿��ؼ� ��ã�⸦ �ϰ�ʹ�.
//����, ���� ���·� �����ϰ� �ʹ�.
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

    //������ �ε��� ��ȣ
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
    public float detectedRadius = 5; //��������
    GameObject chaseTarget;

    private void UpdatePatrol()
    {
        //�ֺ� �ݰ� 5M�ȿ� Player�� �ִٸ�
        Collider[] cols = Physics.OverlapSphere(transform.position, detectedRadius);
        for (int i = 0; i < cols.Length; i++) //�ݶ��̴��� ��� �迭�� �ִٰ�
        {
            if (cols[i].name.Contains("Player"))//�ݶ��̴��� ������ �ִ� �� �� �÷��̾��� �̸��� ���� ��
            {
                //���� ���·� �����ϰ� �ʹ�.
                state = State.Chase;
                chaseTarget = cols[i].gameObject; //ã�� ���� ����������
                return;
            }
        }

        Vector3 target = SY_PathInfo.instance.wayPoints[targetIndex].transform.position;
        //���� ��ȯ �̵��ϰ� �ʹ�.
        agent.destination = target;
        //���� �����ߴٸ�? ->�Ÿ���
        float distance = Vector3.Distance(target, transform.position);
        if (distance <= 2) //1�̶�� �Ÿ��� ��Ȳ�� �°� ���� ���ؾ��Ѵ�.
        {
            //�ε����� 1����.
            anim.SetTrigger("Move");
            targetIndex++;
            //���� targetIndex�� PathInfo.instance.wayPoints�迭 ũ�� �̻��̶�� 0���� �ʱ�ȭ
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
        agent.destination = chaseTarget.transform.position; //Agent�� �������� ã�� �÷��̾�Ÿ������!
        //���� chaseTarget���� �Ÿ��� ���������� �����
        float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (distance > chaseDistance)
        {
            //�������·� �����ϰ�ʹ�.
            state = State.Patrol;
        }
        //���� ���������� �Ÿ��� <=  ���ݰ��ɰŸ����?
        float enemyAttackDistance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (enemyAttackDistance <= attackDistance)
        {
            //���ݻ��·� �����ϰ�ʹ�.
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
        //���� ���ݰ��ɰŸ���� Hit�� �ϰ�ʹ�.
        float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (distance <= agent.stoppingDistance)
        {
            print("hit!!!"); //���� �����̴����� ���� ����!!
            SY_HitManager.instance.DoHitPlz();
        }

    }
    public void OnEnemyAttackFinished()
    {
        print("OnEnemyAttackFinished");
        //���� ���ݰ��ɰŸ��� �ƴ϶��
        float distance = Vector3.Distance(chaseTarget.transform.position, transform.position);
        if (distance > agent.stoppingDistance)
        {
            //�������·� �����ϰ� �ʹ�.
            state = State.Chase;
            anim.SetTrigger("Move");

        }

    }

    //Player => Enemy������
    // ������������ �Ű����� �Ѱܹޱ�.
    public void TryDamage(int damageValue)
    {
        enemyHP.HP -= damageValue;
        if (enemyHP.HP <= 0)
        {
            //�״� �ִϸ��̼��� ������
            //Destroy
            Destroy(gameObject);
        }
        else
        {
            //�˹� �� �ణ�� ����,,?

        }

    }


}
