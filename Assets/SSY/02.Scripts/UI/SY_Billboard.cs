using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶��� ȸ������� ��ġ�����ֱ�.
public class SY_Billboard : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
