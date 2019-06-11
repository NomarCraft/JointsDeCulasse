using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{
    public GameObject pressStart;
    public bool active;
   
    void Update()
    {
        if (Input.GetButton("Start1"))
        {
            if (active == true)
            {
                pressStart.SetActive(true);
            }
            else
            {
                pressStart.SetActive(false);
            }
        }
        
    }
}
