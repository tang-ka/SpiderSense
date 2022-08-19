using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_EnemyHP : MonoBehaviour
{
    //3번 키 누를때 마다 1씩 줄여서 0보다 작거나 작아지면 나를 파괴해라
    //필요속성 : HP
    int hp = 3;
    
   
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //3번 키를 누르면
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            //hp가 1 작아지고
            hp = hp - 1;
            
        //hp<= 0 되면
        if(hp <= 0)
            {
                //자신을 파괴
                Destroy(gameObject);

            }

        }
    }
}
