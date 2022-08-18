using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float curTime1;
    float curTime2;

    float creatTime = 8;
    float ccreatTime = 2;
    // Update is called once per frame
    void Update()
    {
        // 8초가 지나면 8을 출력한다.
        curTime1 += Time.deltaTime; //시간 흐름 
        if(curTime1 > creatTime)
        {
            print("8"); // 8출력
            
            // 그 후 2초가  지나면 10을 출력한다.
            curTime2 += Time.deltaTime;
            if(curTime2 > ccreatTime)
            {
                print("2"); // 2출력
            }
        }
    }
}
