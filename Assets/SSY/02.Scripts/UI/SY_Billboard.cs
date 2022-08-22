using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라의 회전방향과 일치시켜주기.
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
