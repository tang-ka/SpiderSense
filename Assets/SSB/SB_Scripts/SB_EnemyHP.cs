using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SB_EnemyHP : MonoBehaviour
{
    //3�� Ű ������ ���� 1�� �ٿ��� 0���� �۰ų� �۾����� ���� �ı��ض�
    //�ʿ�Ӽ� : HP
    public int hp = 3;

    //HP �����̴� ���ӿ�����Ʈ
    public GameObject HP_red;

    //Image ������Ʈ
    Image hpRedImage;

    public static SB_EnemyHP Instance;
    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        //HP_red ���ӿ�����Ʈ���� Image������Ʈ�� ��������.
        hpRedImage = HP_red.GetComponent<Image>();
    }

    public void OnDamageHP()
    {
        //hp�� 1 �۾�����
        hp = hp - 1;
        //hp���� �̿��ؼ� fillAmount���� ����(��ʽ�)
        //���� Hp / HP�ִ밪
        hpRedImage.fillAmount = hp / 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //3�� Ű�� ������
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    //hp�� 1 �۾�����
        //    hp = hp - 1;
        //    //hp���� �̿��ؼ� fillAmount���� ����(��ʽ�)
        //    //���� Hp / HP�ִ밪
        //    hpRedImage.fillAmount = hp / 3.0f;

        //    //hp<= 0 �Ǹ�
        //    if (hp <= 0)
        //    {
        //        //�ڽ��� �ı�
        //        //Destroy(gameObject);

        //    }
        //}
    }
}
