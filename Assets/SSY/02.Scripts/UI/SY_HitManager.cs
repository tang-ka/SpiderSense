using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ImageHit ���ӿ�����Ʈ�� �����ٰ� 0.1�� �Ŀ� �Ⱥ��̰� �ϴ� ����� ����� �ʹ�.
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
    //ImageHit���ӿ�����Ʈ�� �����ٰ� 0.1�� �Ŀ� �Ⱥ��̰� �ϴ� ����� ����� �ʹ�.
    public void DoHitPlz()
    {
        StopCoroutine("IEDoHit");//�̹� �������̶��!
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
