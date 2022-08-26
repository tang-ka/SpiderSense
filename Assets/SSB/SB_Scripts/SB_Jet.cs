
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SB_Jet : MonoBehaviour
{
    public Transform target;

    #region 상태정의
    public enum EnemyState
    {
        Idle,
        Move,
        RandomMove,
        Attack,     
        Damage,
        Die
    };

    public EnemyState e_state = EnemyState.Idle; //초기값을 Idle로 셋팅했다
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dir = transform.right;
    }
    // Update is called once per frame
    void Update()

    //switch문은 switch문과 case또는 default를 이용해서,switch에서 얻은 값을 통해서 case를 비교해서
    //있다면 해당 case문 안에 있는 코드를 실행하고 없다면 default문을 실행하는 조건문이다 
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
        //BigBulletManager 활성화 시킨다.
        bigBulletManager.SetActive(true);
        //smallBullets 에서 하나씩 돌면서
        for(int i = 0; i < smallBullets.Length; i++)
        {
            //SB_Small_Bullet 컴포넌트를 가져온다.
            SB_Small_Bullet smallBullet = smallBullets[i].GetComponent<SB_Small_Bullet>();
            //가져온 컴포넌트에서 Fire 함수를 실행
            smallBullet.Fire();
        }
    }

    // 타겟이 일정거리에 들어오면 Idle상태에서 Move로 전환된다.
    #region Idle 속성
    // 필요속성 : 거리 -> 타겟과의 거리, 감지 거리
    float distance;  //계속 이동하는 중
    public float moveDistance = 40; // 감지 거리 
    #endregion

    //필요속성 : 감지거리 (정해진 - Move > Idle)

    //필요속성 : Jet 본체 
    public Transform jetModel; 

    private void Idle()
    {
        // 1. 타겟과의 거리
        distance = Vector3.Distance(target.position, transform.position);
        // 2. 일정 거리안에 타겟이 들왔으니까
        if (distance <= moveDistance)
        {
            // 3. Idle상태에서 Move상태로 전환하고 싶다.
            e_state = EnemyState.Move;
        }

        //1초가 지나면 Idle상태에서 RandomMove or BigBulletAttack or SmallBulletAttack 로 전환하고싶다.
        //1.시간이 흐른다.
        curTime += Time.deltaTime;
        //2.일정시간이 현재시간을 초과하면
        if (curTime > createRTime)
        {
            //3.Idle에서 RandomMove or BigBulletAttack or SmallBulletAttack 로 전환
            // 랜던값을 구한다.
            int rand = Random.Range(0, 3);
            //만약에 rand 가 0 이라면 상태를 RandomMove로
            if(rand == 0)
            {
                e_state = EnemyState.RandomMove;

                //방향 바꿀때마다 45도 왼쪽 오른쪽 바꾸기 
                if(dir == transform.right)
                {
                    jetModel.transform.localEulerAngles = new Vector3(0,0,1) * -45;
                }

                if (dir == -transform.right)
                {
                    jetModel.transform.localEulerAngles = new Vector3(0, 0, 1) * 45;
                }



                //4. 이동할 방향을 바꿔준다.
                dir = -dir;
                createRTime = 1;

                {
                    
                }
            }
            //그렇지 않고 만약에 rand 가 1 이라면 상태를 BigBulletAttack로
            else if(rand == 1)
            {
                BigBulletAttack();
                createRTime = 3;
            }
            //그렇지 않으면 상태를 SmallBulletAttack 로
            else if(rand ==2)
            {
                SmallBulletAttack();
                createRTime = 2;
            }
                        
            curTime = 0;
        }
    }

    //필요속성 : 현재시간, 일정시간

    float createRTime = 1;
    Vector3 dir;

    private void RandomMove()
    {
        //1초가 지나면 왼쪽으로 움직이고 싶다.
        //1.시간이 흐른다.
        curTime += Time.deltaTime;

        //2.creatRTime이 currentTime을 초과하면
        if (curTime > createRTime)
        {
            e_state = EnemyState.Idle;
            // 총알 한발을 나가게 하고 싶다. 
            // 1. 총알을 생성한다.
            GameObject bullet = Instantiate(bulletFactory);

            //앞 방향이 타겟을 향하게 하고싶다.
            //transform.forward = target.transform.position - firePosition.transform.position;
            //총알 위치를 총알공장 위치로 가져다 놔야한다.
            bullet.transform.position = firePosition.transform.position;
            //총알 앞 방향과 FirePosition의 앞 방향을 일치시키고 싶다.
            bullet.transform.forward = firePosition.transform.forward;
            curTime = 0;
        }

        //(else)그렇지않다면 오른쪽으로 이동하고 싶다. 
        else 
        {
            //Vector3 dir = transform.right;
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    //Enemy가 Target 방향으로 이동한다. 일정거리 안으로 들어오면 Move > Attack
    //필요속성 : 이동속도 
    public float speed = 5;
    //필요속성 : 감지거리(정해진)
    public float attackDistance = 30;
    public float idleDistance = 60;

    private void Move()
    {
        //2.타겟 방향으로
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
        dir.Normalize();
        //3.이동하고싶다.
        //P = P0+vt
        transform.position += dir * speed * Time.deltaTime;

        //Move>Attack

        // 4. 타겟과의 거리
        Vector3 jetGround = transform.position;
        jetGround.y = 0;
        distance = Vector3.Distance(target.position, jetGround);
        //4. 일정거리 안으로 들어오면
        if (distance <= attackDistance)
        {
            //5.Move > Attack
            e_state = EnemyState.Attack;
        }

        //6.Move > Idle
        //1.일정 거리 이상 타겟과 멀어지면 
        if (distance >= idleDistance)
        {
            //2.Move에서 Idle상태로 전환한다.
            e_state = EnemyState.Idle;
        }

    }

    //필요속성 : 감지거리 (정해진 - move로 바뀔)

    private void Attack()
    {
        Fire();

        //1.일정거리 이상 멀어지면
        if (distance >= attackDistance)
        {
            //2.move상태로 바뀐다.
            e_state = EnemyState.Move;
        }
    }


    public GameObject bulletFactory;
    public GameObject firePosition;

    // 타겟방향으로 총알이 나가게 한다.
      
    // 일정 시간마다 총알을 발사하고 싶다.
    // 필요속성 : 현재시간, 생성시간
    float curTime;
    public float createTime = 2;

    void Fire() 
    {
        // 1. 시간이 흐른다.
        curTime += Time.deltaTime;
        // 2. 현재시간이 생성시간을 초과한다.
        if (curTime > createTime)
        {
            // 3. 총알을 생성한다.
            //총알 프리팹을 총알공장에 담았다.
            GameObject bullet = Instantiate(bulletFactory);

            //앞 방향이 타겟을 향하게 하고싶다.
            transform.forward = target.transform.position - firePosition.transform.position;
            //총알 위치를 총알공장 위치로 가져다 놔야한다.
            bullet.transform.position = firePosition.transform.position;
            //총알 앞 방향과 FirePosition의 앞 방향을 일치시키고 싶다.
            bullet.transform.forward = firePosition.transform.forward;
          

            curTime = 0;

        }
    }

    //피격 이벤트를 받아서 처리 함수
    //필요속성 : 체력
    public int hp = 3;
    
    public void OnDamageProcess()
    {
        hp--;
        //체력이 없으면 상태를 죽음으로 전환 
        if(hp <=0)
        {
            e_state = EnemyState.Die;
        }

        //체력이 남아있으면 상태를 피격으로 전환하고 싶다.
        else
        {
            e_state = EnemyState.Damage;
        }

    }

    //일정시간 기다렸다가 대기로 전환하고 싶다.
    //필요속성 : 피격대기시간 
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
        //시네머신이 재생된다
    }

    
}                                       