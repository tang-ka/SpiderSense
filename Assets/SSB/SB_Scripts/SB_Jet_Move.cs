using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Jet_Move : MonoBehaviour
{

    //Jet�� �÷��̾��� �ֺ��� ���ƴٴϰ� �ϰ�ʹ�.
    //�������� ���ϰ�,Player�� position�� �������� ���Ѵ�
    //������
    float randomic = Random.Range(0,10);
    //�÷��̾��� Position��
    Vector3 playerPos = GameObject.Find("Player").transform.position;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //randomic���� �÷��̾� �����ǿ� ���ؼ� ���簪�� �־��ش�
        
        
    }
}
