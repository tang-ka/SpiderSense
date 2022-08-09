using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Bullet : MonoBehaviour
{
    //총알이 앞으로 나가게 하고싶다.
    //필요속성 : 속도
    public float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //총알이 앞으로 나가게 하고싶다.
        //1.방향이 필요하다 (앞)
        Vector3 dir = transform.forward;
        dir.Normalize();
        //2.이동 하고싶다.
        //P = P0 +vt
        transform.position += dir * speed * Time.deltaTime;
    }
}
