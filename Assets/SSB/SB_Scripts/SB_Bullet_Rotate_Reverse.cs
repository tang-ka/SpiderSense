using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet�� Ư�������� ȸ����Ű�� �ʹ�.

public class SB_Bullet_Rotate_Reverse : MonoBehaviour
{
    //�ʿ�Ӽ� rotSpeed
    public float rotspeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -1) * rotspeed * Time.deltaTime) ;
    }
}
