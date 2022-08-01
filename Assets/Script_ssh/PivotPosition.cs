using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotPosition : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + Vector3.up * 0.5f;
    }
}
