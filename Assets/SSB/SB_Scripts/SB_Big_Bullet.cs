using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Big_Bullet : MonoBehaviour
{
    //비활성화 된 상태로 시작, 1번 버튼을 누르면 BigBulletManager 실행

    //총알이 앞으로 나가게 하고싶다.
    //필요속성 : 속도 
    public float speed = 30;
    //필요속성 : 타겟포지션
    public Transform target;
    //필요속성 : BigBulletFManager의 처음 생성됐을때 위치 
    Vector3 initialPosition;
    //필요 속성 : BulletPosition의 위치 (BigBulletManager의 빈 오브젝트)
    public GameObject BulletPosition;
    //Vector3 BulletPosition;

    bool isKeyDown1;
    bool isComeBack;
    float curTime;
    public float creatTime = 1; 

    public GameObject BigBulletFactory;
    

    // Start is called before the first frame update
    void Start()
    {
        //1.비활성화 된 상태로 시작
        BigBulletFactory.SetActive(false);
        isKeyDown1 = false;
        BulletPosition.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //2.1번 버튼을 누르면 BigBulletManager를 킨다.
        //2-1. 1번 버튼을 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("GetKeyCode : Alpha1");
            //2-2. BigBulletManaer를 킨다.
            BigBulletFactory.SetActive(true);
            //2-2.BulletPosition도 킨다.
            BulletPosition.SetActive(true);

            //BigBulletFactory의 처음 값을 가져온다
            initialPosition = BigBulletFactory.transform.position;  

             


            //2-3. 방향을 타겟쪽으로 >> target - me
            Vector3 dir = target.transform.position - BulletPosition.transform.position;
            dir.Normalize();
            isKeyDown1 = true;
        }
        //앞쪽으로 발사하고 1초가 지나면 멈춘다.

        //1번 키를 눌렀을때 
        if (isKeyDown1)
        {
            curTime += Time.deltaTime;
            Vector3 dir = target.transform.position - BulletPosition.transform.position;
            dir.Normalize();

            BigBulletFactory.transform.position += dir * speed * Time.deltaTime; //앞쪽으로 발사 

            if (curTime > creatTime)
            {
                isKeyDown1 = false;
                isComeBack = true;
                curTime = 0;
            }

        }

        //반대방향 dir

        else if (isComeBack == true) //일정시간이 되면 
        {
            //반대방향으로 간다.
            //1.거리를 구한다. 불렛포지션 위치랑 타겟 위치 
            Vector3 dir = BulletPosition.transform.position - BigBulletFactory.transform.position;

           //이동한다 
           BigBulletFactory.transform.position += dir.normalized * speed * Time.deltaTime;

            float moveDis = Vector3.Distance(BulletPosition.transform.position, BigBulletFactory.transform.position);

            print(moveDis);
            //2.거리가 좁혀지면
            float distance = 0.5f;
            if( moveDis < distance )
            {
                //3.거리와 BigBullet거리가 같아진다.
                BigBulletFactory.transform.position = BulletPosition.transform.position;
                isComeBack = false;

            }

            
        }
    }
}
