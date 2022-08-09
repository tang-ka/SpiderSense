using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Jet_Move : MonoBehaviour
{

    //Jet가 플레이어의 주변을 날아다니게 하고싶다.
    //랜덤값을 정하고,Player의 position에 랜덤값을 더한다
    //랜덤값
    float randomic = Random.Range(0,10);
    //플레이어의 Position값
    Vector3 playerPos = GameObject.Find("Player").transform.position;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //randomic값을 플레이어 포지션에 더해서 현재값에 넣어준다
        
        
    }
}
