using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//2번을 누르면 BigBullet에서 총알이 나가게 한다.
//각 Big_Bullet 4개에 이 스크립트를 다 붙일것이다.
//동일한 조건에 다 발사될 수 있도록 하기 

//필요속성 : SmallBulletFactory

public class SB_Small_Bullet : MonoBehaviour
{
    //필요속성 : SmallBulletFactory
    public GameObject SmallBulletFactory;

    //필요속성 : 속도
    public float speed = 5;

    //필요속성 : PlayerPos
    public Transform target;

    //맨첨엔 총알이 안보여야 하니까 비활성화 된 상태로 시작
    void Start()
    {
        
    }

    public void Fire()
    {
        //2. Bullet 을 데려와서 (prefab)
        GameObject SBullet = Instantiate(SmallBulletFactory);
        //3. 발사시킨다.
         
        // smaillBullet을 bigBullet자리에 가져다 놓는다
        SBullet.transform.position = transform.position;

        // 작은 총알 앞 방향과 큰 총알의 앞 방향을 일치시키고 싶다.
        SBullet.transform.forward = transform.forward;

        //앞 방향이 타겟을 향하게 하고싶다.
        transform.forward = target.transform.position - transform.position;

        //4-2.앞 방향이 타겟을 향하게 하고싶다.
        Vector3 dir = target.transform.position - transform.position;

        dir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        //1. 2번 버튼을 누르면 
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fire();
        }
    }
}
