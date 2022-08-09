using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알생산공장 
//총알 프리팹을 가져와서 계속 앞으로 나가게 하고 싶다.
public class SB_EnemyFire : MonoBehaviour
{

    public GameObject bulletFactory;
    public GameObject firePosition;

    //타겟방향으로 총알이 나가게 한다.
    //타겟
    public GameObject target;

    // 일정 시간마다 총알을 발사하고 싶다.
    // 필요속성 : 현재시간, 생성시간
    float curTime;
    public float createTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1. 시간이 흐른다.
        curTime += Time.deltaTime;
        // 2. 현재시간이 생성시간을 초과한다.
        if (curTime > createTime)
        {
            // 3. 총알을 생성한다.
            //총알 프리팹을 총알공장에 담았다.
            GameObject bullet = Instantiate(bulletFactory);

            //총알 위치를 총알공장 위치로 가져다 놔야한다.
            bullet.transform.position = firePosition.transform.position;
            //총알 앞 방향과 FirePosition의 앞 방향을 일치시키고 싶다.
            bullet.transform.forward = firePosition.transform.forward;

            curTime = 0;
        }
    }
}

