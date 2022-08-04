using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_SpiderMove : MonoBehaviour
{
    public Transform hook;
    public Transform rightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 webDir = hook.position - rightHand.position;
            float webLen = Vector3.Distance(rightHand.position, hook.position);
            Ray web = new Ray(rightHand.position, webDir);
            Debug.DrawRay(web.origin, web.direction * webLen, Color.white);
            print(111);
        }
    }
}
