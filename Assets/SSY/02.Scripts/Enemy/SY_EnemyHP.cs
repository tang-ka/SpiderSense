using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ü���� ǥ���ϰ� �ʹ�.
public class SY_EnemyHP : MonoBehaviour
{
    int hp;
    public int maxHP;
    public Slider sliderHp;
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            sliderHp.value = hp;
        }
    }
    void Start()
    {
        //���� �� ü���� �ִ�ü������
        sliderHp.maxValue = maxHP;
        HP = maxHP;
    }

    void Update()
    {

    }
}
