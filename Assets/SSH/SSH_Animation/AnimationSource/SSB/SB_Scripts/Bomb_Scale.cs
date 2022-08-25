using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Scale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //크기를 4배해서 크기를 키운다.
        transform.localScale *= 600f;
    }
}
