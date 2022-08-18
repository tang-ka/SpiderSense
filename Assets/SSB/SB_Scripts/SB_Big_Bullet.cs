using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Big_Bullet : MonoBehaviour
{
    //��Ȱ��ȭ �� ���·� ����, 1�� ��ư�� ������ BigBulletManager ����

    //�Ѿ��� ������ ������ �ϰ�ʹ�.
    //�ʿ�Ӽ� : �ӵ� 
    public float speed = 30;
    //�ʿ�Ӽ� : Ÿ��������
    public Transform target;
    //�ʿ�Ӽ� : BigBulletFManager�� ó�� ���������� ��ġ 
    Vector3 initialPosition;
    //�ʿ� �Ӽ� : BulletPosition�� ��ġ (BigBulletManager�� �� ������Ʈ)
    public GameObject BulletPosition;
    //Vector3 BulletPosition;

    bool isKeyDown1;
    bool isComeBack;
    float curTime;
    public float creatTime = 1; 

    public GameObject BigBulletFactory;
    

    // Start is called before the first frame update
    void Start()
    {
        //1.��Ȱ��ȭ �� ���·� ����
        BigBulletFactory.SetActive(false);
        isKeyDown1 = false;
        BulletPosition.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //2.1�� ��ư�� ������ BigBulletManager�� Ų��.
        //2-1. 1�� ��ư�� ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("GetKeyCode : Alpha1");
            //2-2. BigBulletManaer�� Ų��.
            BigBulletFactory.SetActive(true);
            //2-2.BulletPosition�� Ų��.
            BulletPosition.SetActive(true);

            //BigBulletFactory�� ó�� ���� �����´�
            initialPosition = BigBulletFactory.transform.position;  

             


            //2-3. ������ Ÿ�������� >> target - me
            Vector3 dir = target.transform.position - BulletPosition.transform.position;
            dir.Normalize();
            isKeyDown1 = true;
        }
        //�������� �߻��ϰ� 1�ʰ� ������ �����.

        //1�� Ű�� �������� 
        if (isKeyDown1)
        {
            curTime += Time.deltaTime;
            Vector3 dir = target.transform.position - BulletPosition.transform.position;
            dir.Normalize();

            BigBulletFactory.transform.position += dir * speed * Time.deltaTime; //�������� �߻� 

            if (curTime > creatTime)
            {
                isKeyDown1 = false;
                isComeBack = true;
                curTime = 0;
            }

        }

        //�ݴ���� dir

        else if (isComeBack == true) //�����ð��� �Ǹ� 
        {
            //�ݴ�������� ����.
            //1.�Ÿ��� ���Ѵ�. �ҷ������� ��ġ�� Ÿ�� ��ġ 
            Vector3 dir = BulletPosition.transform.position - BigBulletFactory.transform.position;

           //�̵��Ѵ� 
           BigBulletFactory.transform.position += dir.normalized * speed * Time.deltaTime;

            float moveDis = Vector3.Distance(BulletPosition.transform.position, BigBulletFactory.transform.position);

            print(moveDis);
            //2.�Ÿ��� ��������
            float distance = 0.5f;
            if( moveDis < distance )
            {
                //3.�Ÿ��� BigBullet�Ÿ��� ��������.
                BigBulletFactory.transform.position = BulletPosition.transform.position;
                isComeBack = false;

            }

            
        }
    }
}
