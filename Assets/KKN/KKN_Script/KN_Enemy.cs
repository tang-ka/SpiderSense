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
        //target�� �����Ÿ��ȿ� ������ Move�� ����
        //1.���� target�� �Ÿ�����
        float distance = Vector3.Distance(transform.position, target.transform.position);
        //2.���� �� �Ÿ��� �����Ÿ����� �۴ٸ�
        if(distance < findDistance)
        {
            //3. move���·� ����
            state = State.Move;
        }
    }

    public float speed = 1;
    public float attackDistance = 1.5f;
    private void UpdateMove()
    {
        // target�������� �̵��ϴٰ� target�� ���ݰŸ� �ȿ� ���´ٸ� attack���� ����
        // 1.target�������� �̵��ϰ�ʹ�.
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
        // 2.���� target�� �Ÿ�����
        float distance = Vector3.Distance(transform.position, target.transform.position);
        // 3.�� �Ÿ��� ���ݰŸ����� �۴ٸ� 
        if (distance < attackDistance)
        {
        // 4.attack���� ����
            state = State.Attack1;
        }
    }

    float currentTime;
    float attackTime = 1;
    private void UpdateAttack1()
    {
        //�����ð����� �����ϵ� ���ݽ����� target�� ���ݰŸ� �ۿ� �ִٸ� move��, �ƴ϶�� ��� ����
        //1. �ð����帣�ٰ�
        currentTime += Time.deltaTime;
        //2. ���� ����ð��� ���ݽð��� �Ǹ�
        if(currentTime > attackTime)
        {
            //3. ����ð��� �ʱ�ȭ�ϰ�
            currentTime = 0;
            //4. �÷��̾ �����ϰ�
            //target.AddDamage();
            //5.1 ���� target�� �Ÿ�����
            float distance = Vector3.Distance(transform.position, target.transform.position);
            //5.2 �� �Ÿ��� ���ݰŸ����� ũ�ٸ�
            if(distance > attackDistance)
            {
                //5.3 Move���� ����
                state = State.Move;
            }
        }

    }

    private void UpdateAttack2()
    {
        currentTime += Time.deltaTime;



    }



    //player�� AddDamage �߰��ϱ�
    //public void AddDamage(int damage)
    //{
    //    Destroy(gameObject);
    //}
}
