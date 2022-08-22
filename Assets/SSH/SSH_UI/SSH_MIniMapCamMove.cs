using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSH_MIniMapCamMove : MonoBehaviour
{
    public Transform player;
    Vector3 camPosition;
    
    public float height = 200;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camPosition = player.position;
        camPosition.y = height;
        

        transform.position = camPosition;
        transform.eulerAngles = new Vector3(90, 0, -player.eulerAngles.y);
    }
}
