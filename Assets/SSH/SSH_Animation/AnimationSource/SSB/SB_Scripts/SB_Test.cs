using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Test : MonoBehaviour
{
    public GameObject sphereFactory;

    //�θ���
    public Transform trParent;
    void Start()
    {
        
    }

    void Update()
    {
        //1��Ű ������
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //���� �߽ɿ��� ��������, ũ�Ⱑ ����
            Vector3 dir = Random.insideUnitSphere * Random.Range(1.0f, 5.0f);
            //���׶�̸� �����Ѵ�.
            GameObject sphere = Instantiate(sphereFactory);
            //������ ���׶�̸� �� �߽����� dir���⿡ ��ġ ��Ų��.
            sphere.transform.position = transform.position + dir;
            //������ ���׶�̸� �θ� trParent �� �Ѵ�.
            sphere.transform.parent = trParent;


            //ũ�⸦ 0.2���ؼ� ũ�⸦ ���δ�.
            //sphere.transform.localScale *= 0.2f;
        }
    }
}
