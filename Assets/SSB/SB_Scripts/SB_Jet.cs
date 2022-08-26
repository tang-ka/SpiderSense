
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SB_Jet : MonoBehaviour
{
    public Transform target;

    #region ��������
    public enum EnemyState
    {
        Idle,
        Move,
        RandomMove,
        Attack,     
        Damage,
        Die
    };

    public EnemyState e_state = EnemyState.Idle; //�ʱⰪ�� Idle�� �����ߴ�
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dir = transform.right;
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
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void BigBulletAttack()
    {
        SB_Big_Bullet sbBigBullet = gameObject.GetComponent<SB_Big_Bullet>();
        sbBigBullet.Fire();
    }

    public GameObject bigBulletManager;
    public GameObject[] smallBullets;
    void SmallBulletAttack()
    {
        //BigBulletManager Ȱ��ȭ ��Ų��.
        bigBulletManager.SetActive(true);
        //smallBullets ���� �ϳ��� ���鼭
        for(int i = 0; i < smallBullets.Length; i++)
        {
            //SB_Small_Bullet ������Ʈ�� �����´�.
            SB_Small_Bullet smallBullet = smallBullets[i].GetComponent<SB_Small_Bullet>();
            //������ ������Ʈ���� Fire �Լ��� ����
            smallBullet.Fire();
        }
    }

    // Ÿ���� �����Ÿ��� ������ Idle���¿��� Move�� ��ȯ�ȴ�.
    #region Idle �Ӽ�
    // �ʿ�Ӽ� : �Ÿ� -> Ÿ�ٰ��� �Ÿ�, ���� �Ÿ�
    float distance;  //��� �̵��ϴ� ��
    public float moveDistance = 40; // ���� �Ÿ� 
    #endregion

    //�ʿ�Ӽ� : �����Ÿ� (������ - Move > Idle)

    //�ʿ�Ӽ� : Jet ��ü 
    public Transform jetModel; 

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

        //1�ʰ� ������ Idle���¿��� RandomMove or BigBulletAttack or SmallBulletAttack �� ��ȯ�ϰ�ʹ�.
        //1.�ð��� �帥��.
        curTime += Time.deltaTime;
        //2.�����ð��� ����ð��� �ʰ��ϸ�
        if (curTime > createRTime)
        {
            //3.Idle���� RandomMove or BigBulletAttack or SmallBulletAttack �� ��ȯ
            // �������� ���Ѵ�.
            int rand = Random.Range(0, 3);
            //���࿡ rand �� 0 �̶�� ���¸� RandomMove��
            if(rand == 0)
            {
                e_state = EnemyState.RandomMove;

                //���� �ٲܶ����� 45�� ���� ������ �ٲٱ� 
                if(dir == transform.right)
                {
                    jetModel.transform.localEulerAngles = new Vector3(0,0,1) * -45;
                }

                if (dir == -transform.right)
                {
                    jetModel.transform.localEulerAngles = new Vector3(0, 0, 1) * 45;
                }



                //4. �̵��� ������ �ٲ��ش�.
                dir = -dir;
                createRTime = 1;

                {
                    
                }
            }
            //�׷��� �ʰ� ���࿡ rand �� 1 �̶�� ���¸� BigBulletAttack��
            else if(rand == 1)
            {
                BigBulletAttack();
                createRTime = 3;
            }
            //�׷��� ������ ���¸� SmallBulletAttack ��
            else if(rand ==2)
            {
                SmallBulletAttack();
                createRTime = 2;
            }
                        
            curTime = 0;
        }
    }

    //�ʿ�Ӽ� : ����ð�, �����ð�

    float createRTime = 1;
    Vector3 dir;

    private void RandomMove()
    {
        //1�ʰ� ������ �������� �����̰� �ʹ�.
        //1.�ð��� �帥��.
        curTime += Time.deltaTime;

        //2.creatRTime�� currentTime�� �ʰ��ϸ�
        if (curTime > createRTime)
        {
            e_state = EnemyState.Idle;
            // �Ѿ� �ѹ��� ������ �ϰ� �ʹ�. 
            // 1. �Ѿ��� �����Ѵ�.
            GameObject bullet = Instantiate(bulletFactory);

            //�� ������ Ÿ���� ���ϰ� �ϰ�ʹ�.
            //transform.forward = target.transform.position - firePosition.transform.position;
            //�Ѿ� ��ġ�� �Ѿ˰��� ��ġ�� ������ �����Ѵ�.
            bullet.transform.position = firePosition.transform.position;
            //�Ѿ� �� ����� FirePosition�� �� ������ ��ġ��Ű�� �ʹ�.
            bullet.transform.forward = firePosition.transform.forward;
            curTime = 0;
        }

        //(else)�׷����ʴٸ� ���������� �̵��ϰ� �ʹ�. 
        else 
        {
            //Vector3 dir = transform.right;
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
        Fire();

        //1.�����Ÿ� �̻� �־�����
        if (distance >= attackDistance)
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

            //�� ������ Ÿ���� ���ϰ� �ϰ�ʹ�.
            transform.forward = target.transform.position - firePosition.transform.position;
            //�Ѿ� ��ġ�� �Ѿ˰��� ��ġ�� ������ �����Ѵ�.
            bullet.transform.position = firePosition.transform.position;
            //�Ѿ� �� ����� FirePosition�� �� ������ ��ġ��Ű�� �ʹ�.
            bullet.transform.forward = firePosition.transform.forward;
          

            curTime = 0;

        }
    }

    //�ǰ� �̺�Ʈ�� �޾Ƽ� ó�� �Լ�
    //�ʿ�Ӽ� : ü��
    public int hp = 3;
    
    public void OnDamageProcess()
    {
        hp--;
        //ü���� ������ ���¸� �������� ��ȯ 
        if(hp <=0)
        {
            e_state = EnemyState.Die;
        }

        //ü���� ���������� ���¸� �ǰ����� ��ȯ�ϰ� �ʹ�.
        else
        {
            e_state = EnemyState.Damage;
        }

    }

    //�����ð� ��ٷȴٰ� ���� ��ȯ�ϰ� �ʹ�.
    //�ʿ�Ӽ� : �ǰݴ��ð� 
    public float damageDelayTime = 2; 
  
    private void Damage()
    {
        curTime += Time.deltaTime;
        if(curTime > damageDelayTime)
        {
            e_state = EnemyState.Idle;
            curTime = 0;
        }

    }

    private void Die()
    {
        //�ó׸ӽ��� ����ȴ�
    }

    
}                                       