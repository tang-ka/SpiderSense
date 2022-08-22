using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ImageHit 게임오브젝트를 보였다가 0.1초 후에 안보이게 하는 기능을 만들고 싶다.
public class SY_HitManager : MonoBehaviour
{
    public static SY_HitManager instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject imageHit;
    void Start()
    {
        imageHit.SetActive(false);
    }
    //ImageHit게임오브젝트를 보였다가 0.1초 후에 안보이게 하는 기능을 만들고 싶당.
    public void DoHitPlz()
    {
        StopCoroutine("IEDoHit");//이미 실행중이라면!
        StartCoroutine("IEDoHit");
    }
    IEnumerator IEDoHit()
    {
        imageHit.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        imageHit.SetActive(false);
    }
    void Update()
    {

    }
}
