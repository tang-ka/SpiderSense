using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Bullet : MonoBehaviour
{
    //�Ѿ��� ������ ������ �ϰ�ʹ�.
    //�ʿ�Ӽ� : �ӵ�
    public float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�Ѿ��� ������ ������ �ϰ�ʹ�.
        //1.������ �ʿ��ϴ� (��)
        Vector3 dir = transform.forward;
        dir.Normalize();
        //2.�̵� �ϰ�ʹ�.
        //P = P0 +vt
        transform.position += dir * speed * Time.deltaTime;
    }
}
