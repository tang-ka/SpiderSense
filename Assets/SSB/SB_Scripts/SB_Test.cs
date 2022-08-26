using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Test : MonoBehaviour
{
    public GameObject sphereFactory;

    //부모역할
    public Transform trParent;
    void Start()
    {
        
    }

    void Update()
    {
        //1번키 누르면
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //원점 중심에서 랜덤방향, 크기가 랜덤
            Vector3 dir = Random.insideUnitSphere * Random.Range(1.0f, 5.0f);
            //동그라미를 생성한다.
            GameObject sphere = Instantiate(sphereFactory);
            //생성된 동그라미를 나 중심으로 dir방향에 위치 시킨다.
            sphere.transform.position = transform.position + dir;
            //생성된 동그라미를 부모를 trParent 로 한다.
            sphere.transform.parent = trParent;


            //크기를 0.2배해서 크기를 줄인다.
            //sphere.transform.localScale *= 0.2f;
        }
    }
}
