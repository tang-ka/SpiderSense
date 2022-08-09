using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ѿ˻������ 
//�Ѿ� �������� �����ͼ� ��� ������ ������ �ϰ� �ʹ�.
public class SB_EnemyFire : MonoBehaviour
{

    public GameObject bulletFactory;
    public GameObject firePosition;

    //Ÿ�ٹ������� �Ѿ��� ������ �Ѵ�.
    //Ÿ��
    public GameObject target;

    // ���� �ð����� �Ѿ��� �߻��ϰ� �ʹ�.
    // �ʿ�Ӽ� : ����ð�, �����ð�
    float curTime;
    public float createTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 1. �ð��� �帥��.
        curTime += Time.deltaTime;
        // 2. ����ð��� �����ð��� �ʰ��Ѵ�.
        if (curTime > createTime)
        {
            // 3. �Ѿ��� �����Ѵ�.
            //�Ѿ� �������� �Ѿ˰��忡 ��Ҵ�.
            GameObject bullet = Instantiate(bulletFactory);

            //�Ѿ� ��ġ�� �Ѿ˰��� ��ġ�� ������ �����Ѵ�.
            bullet.transform.position = firePosition.transform.position;
            //�Ѿ� �� ����� FirePosition�� �� ������ ��ġ��Ű�� �ʹ�.
            bullet.transform.forward = firePosition.transform.forward;

            curTime = 0;
        }
    }
}

