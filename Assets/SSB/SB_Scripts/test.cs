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
        // 8�ʰ� ������ 8�� ����Ѵ�.
        curTime1 += Time.deltaTime; //�ð� �帧 
        if(curTime1 > creatTime)
        {
            print("8"); // 8���
            
            // �� �� 2�ʰ�  ������ 10�� ����Ѵ�.
            curTime2 += Time.deltaTime;
            if(curTime2 > ccreatTime)
            {
                print("2"); // 2���
            }
        }
    }
}
