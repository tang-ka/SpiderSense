using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//체력을 표현하고 싶다.
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
        //생성 시 체력을 최대체력으로
        sliderHp.maxValue = maxHP;
        HP = maxHP;
    }

    void Update()
    {

    }
}
