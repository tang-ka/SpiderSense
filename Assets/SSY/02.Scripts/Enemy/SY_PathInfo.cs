using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SY_PathInfo : MonoBehaviour
{
    public static SY_PathInfo instance;

    public GameObject[] wayPoints;
    private void Awake()
    {
        instance = this;
    }


}
