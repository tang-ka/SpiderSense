using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSH_CrossHair : MonoBehaviour
{
    public SSH_WebMove wm;
    public GameObject crossHair;

    // Start is called before the first frame update
    void Start()
    {
        crossHair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (wm.EdgeDetection(out hit))
        {
            crossHair.SetActive(true);
            crossHair.transform.position = Camera.main.WorldToScreenPoint(hit.point);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }
}
