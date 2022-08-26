using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//2���� ������ BigBullet���� �Ѿ��� ������ �Ѵ�.
//�� Big_Bullet 4���� �� ��ũ��Ʈ�� �� ���ϰ��̴�.
//������ ���ǿ� �� �߻�� �� �ֵ��� �ϱ� 

//�ʿ�Ӽ� : SmallBulletFactory

public class SB_Small_Bullet : MonoBehaviour
{
    //�ʿ�Ӽ� : SmallBulletFactory
    public GameObject SmallBulletFactory;

    //�ʿ�Ӽ� : �ӵ�
    public float speed = 5;

    //�ʿ�Ӽ� : PlayerPos
    public Transform target;

    //��÷�� �Ѿ��� �Ⱥ����� �ϴϱ� ��Ȱ��ȭ �� ���·� ����
    void Start()
    {
        
    }

    public void Fire()
    {
        //2. Bullet �� �����ͼ� (prefab)
        GameObject SBullet = Instantiate(SmallBulletFactory);
        //3. �߻��Ų��.
         
        // smaillBullet�� bigBullet�ڸ��� ������ ���´�
        SBullet.transform.position = transform.position;

        // ���� �Ѿ� �� ����� ū �Ѿ��� �� ������ ��ġ��Ű�� �ʹ�.
        SBullet.transform.forward = transform.forward;

        //�� ������ Ÿ���� ���ϰ� �ϰ�ʹ�.
        transform.forward = target.transform.position - transform.position;

        //4-2.�� ������ Ÿ���� ���ϰ� �ϰ�ʹ�.
        Vector3 dir = target.transform.position - transform.position;

        dir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        //1. 2�� ��ư�� ������ 
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fire();
        }
    }
}
