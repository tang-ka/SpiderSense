using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet을 특정각도로 회전시키고 싶다.

public class SB_Bullet_Rotate_Reverse : MonoBehaviour
{
    //필요속성 rotSpeed
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
