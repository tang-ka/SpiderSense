using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSH_ImageChange : MonoBehaviour
{
    public Image[] image = new Image[9];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    float currentTime = 0;
    public float endTime = 1;
    // Update is called once per frame
    void Update()
    {
        int imageNum = Random.Range(0, 10);

        currentTime += Time.deltaTime;

        switch (imageNum)
        {
            case 0:
                if (currentTime > endTime)
                {

                    break;
                }
        }
    }
}
