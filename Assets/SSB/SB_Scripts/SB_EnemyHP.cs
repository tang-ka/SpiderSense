using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SB_EnemyHP : MonoBehaviour
{
    //3번 키 누를때 마다 1씩 줄여서 0보다 작거나 작아지면 나를 파괴해라
    //필요속성 : HP
    public int hp = 3;

    //HP 슬라이더 게임오브젝트
    public GameObject HP_red;

    //Image 컴포넌트
    Image hpRedImage;

    public static SB_EnemyHP Instance;
    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        //HP_red 게임오브젝트에서 Image컴포넌트를 가져오자.
        hpRedImage = HP_red.GetComponent<Image>();
    }

    public void OnDamageHP()
    {
        //hp가 1 작아지고
        hp = hp - 1;
        //hp값을 이용해서 fillAmount값을 셋팅(비례식)
        //현재 Hp / HP최대값
        hpRedImage.fillAmount = hp / 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //3번 키를 누르면
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    //hp가 1 작아지고
        //    hp = hp - 1;
        //    //hp값을 이용해서 fillAmount값을 셋팅(비례식)
        //    //현재 Hp / HP최대값
        //    hpRedImage.fillAmount = hp / 3.0f;

        //    //hp<= 0 되면
        //    if (hp <= 0)
        //    {
        //        //자신을 파괴
        //        //Destroy(gameObject);

        //    }
        //}
    }
}
