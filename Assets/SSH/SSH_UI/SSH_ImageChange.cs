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
        int imageNum = Random.Range(0, 9);

        currentTime += Time.deltaTime;

        switch (imageNum)
        {
            case 0:
                ShowImage();
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            case 4:
                break;

            case 5:
                break;

            case 6:
                break;

            case 7:
                break;

            case 8:
                break;
        }
    }

    void ShowImage()
    {

    }
}
