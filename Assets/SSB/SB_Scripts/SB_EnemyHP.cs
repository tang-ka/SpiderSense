using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_EnemyHP : MonoBehaviour
{
    //3�� Ű ������ ���� 1�� �ٿ��� 0���� �۰ų� �۾����� ���� �ı��ض�
    //�ʿ�Ӽ� : HP
    int hp = 3;
    
   
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //3�� Ű�� ������
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            //hp�� 1 �۾�����
            hp = hp - 1;
            
        //hp<= 0 �Ǹ�
        if(hp <= 0)
            {
                //�ڽ��� �ı�
                Destroy(gameObject);

            }

        }
    }
}
