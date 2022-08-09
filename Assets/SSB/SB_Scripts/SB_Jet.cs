using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SB_Jet : MonoBehaviour
{
    public Transform target;
    public enum EnemyState
    {
        Idle,
        Move,
        RandomMove,
        Attack
    };

    public EnemyState e_state = EnemyState.Idle; //�ʱⰪ�� Idle�� �����ߴ�

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()

    //switch���� switch���� case�Ǵ� default�� �̿��ؼ�,switch���� ���� ���� ���ؼ� case�� ���ؼ�
    //�ִٸ� �ش� case�� �ȿ� �ִ� �ڵ带 �����ϰ� ���ٸ� default���� �����ϴ� ���ǹ��̴� 
    {
        Vector3 jetGround = transform.position;
        jetGround.y = 0;
        distance = Vector3.Distance(target.position, jetGround);

        switch (e_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.RandomMove:
                RandomMove();
                break;
            case EnemyState.Attack:
                Attack();
                break;

        }
    }

   

    // Ÿ���� �����Ÿ��� ������ Idle���¿��� Move�� ��ȯ�ȴ�.
    // �ʿ�Ӽ� : �Ÿ� -> Ÿ�ٰ��� �Ÿ�, ���� �Ÿ�
    float distance;  //��� �̵��ϴ� ��
    public float moveDistance = 40; // ���� �Ÿ� 
    
   
    //�ʿ�Ӽ� : �����Ÿ� (������ - Move > Idle)
    
    private void Idle()
    {
        // 1. Ÿ�ٰ��� �Ÿ�
        distance = Vector3.Distance(target.position, transform.position);
        // 2. ���� �Ÿ��ȿ� Ÿ���� ������ϱ�
        if (distance <= moveDistance)
        {
            // 3. Idle���¿��� Move���·� ��ȯ�ϰ� �ʹ�.
            e_state = EnemyState.Move;
        }


        //1�ʰ� ������ Idle���¿��� RandomMove�� ��ȯ�ϰ�ʹ�.
        //1.�ð��� �帥��.
        curTime += Time.deltaTime;
        //2.�����ð��� ����ð��� �ʰ��ϸ�
        if(curTime > createRTime)
        {
            //3.Idle���� RandomMove�� ��ȯ
            e_state = EnemyState.RandomMove;

            curTime = 0;
        }
        

    }



    //�ʿ�Ӽ� : ����ð�, �����ð�

    public float createRTime = 1;
    private void RandomMove()
    {
        //1�ʰ� ������ Idle���·� ��ȯ�ϰ� �ʹ�.

        //1.�ð��� �帥��.
        curTime += Time.deltaTime;

        //2.creatRTime�� currentTime�� �ʰ��ϸ�
        if (curTime > createRTime)
        {

            //3.Idle���·� ��ȯ�Ѵ�
            e_state = EnemyState.Idle;

            curTime = 0;
        }

        //(else)�׷����ʴٸ� ���������� �̵��ϰ� �ʹ�. 
        else 
        {
            Vector3 dir = transform.right;
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    //Enemy�� Target �������� �̵��Ѵ�. �����Ÿ� ������ ������ Move > Attack
    //�ʿ�Ӽ� : �̵��ӵ� 
    public float speed = 5;
    //�ʿ�Ӽ� : �����Ÿ�(������)
    public float attackDistance = 30;
    public float idleDistance = 60; 

    private void Move()
    {



        //2.Ÿ�� ��������
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
        dir.Normalize();
        //3.�̵��ϰ�ʹ�.
        //P = P0+vt
        transform.position += dir * speed * Time.deltaTime;

        //Move>Attack

        // 4. Ÿ�ٰ��� �Ÿ�
        Vector3 jetGround = transform.position;
        jetGround.y = 0;
        distance = Vector3.Distance(target.position, jetGround);
        //4. �����Ÿ� ������ ������
        if (distance <= attackDistance)
        {
            //5.Move > Attack
            e_state = EnemyState.Attack;
        }

        //6.Move > Idle
        //1.���� �Ÿ� �̻� Ÿ�ٰ� �־����� 
        if (distance >= idleDistance)
        {
            //2.Move���� Idle���·� ��ȯ�Ѵ�.
            e_state = EnemyState.Idle;
        }

    }

    //�ʿ�Ӽ� : �����Ÿ� (������ - move�� �ٲ�)
    
    private void Attack()
    {
        //Ÿ�ٹ������� �̵��ϸ鼭 
        /*
        //1.Ÿ�� ��������
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
        dir.Normalize();
        //2.�̵��ϰ�ʹ�.
        //P = P0+vt
        transform.position += dir * speed * Time.deltaTime;
        */
        //Ÿ�ٹ������� �Ѿ��� ������ �Ѵ�.


        //Ÿ�� ������ �ٶ󺸰� �ϰ� �ʹ�.
        // -> �� ������ Ÿ�� �������� �ϰ� �ʹ�.

        
        


        Fire();
        

        //1.�����Ÿ� �̻� �־�����
        if(distance >= attackDistance)
        {
            //2.move���·� �ٲ��.
            e_state = EnemyState.Move;
        }


        
    }




    public GameObject bulletFactory;
    public GameObject firePosition;

    // Ÿ�ٹ������� �Ѿ��� ������ �Ѵ�.
    
    // ���� �ð����� �Ѿ��� �߻��ϰ� �ʹ�.
    // �ʿ�Ӽ� : ����ð�, �����ð�
    float curTime;
    public float createTime = 2;

    void Fire()
    {
        // 1. �ð��� �帥��.
        curTime += Time.deltaTime;
        // 2. ����ð��� �����ð��� �ʰ��Ѵ�.
        if (curTime > createTime)
        {
            // 3. �Ѿ��� �����Ѵ�.
            //�Ѿ� �������� �Ѿ˰��忡 ��Ҵ�.
            GameObject bullet = Instantiate(bulletFactory);

            
            //�Ѿ� ��ġ�� �Ѿ˰��� ��ġ�� ������ �����Ѵ�.
            bullet.transform.position = firePosition.transform.position;
            //�Ѿ� �� ����� FirePosition�� �� ������ ��ġ��Ű�� �ʹ�.
            bullet.transform.forward = firePosition.transform.forward;
            //�� ������ Ÿ���� ���ϰ� �ϰ�ʹ�.
            transform.forward = target.transform.position - firePosition.transform.position;

            curTime = 0;

        }
    }
}
