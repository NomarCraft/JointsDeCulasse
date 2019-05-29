using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penel : MonoBehaviour
{
    public GameObject Panel;

    void Update()
    {
        if(Time.timeScale == 0)
        {
            Panel.gameObject.SetActive(true); 
            Debug.Log("panel");
        }
        else
        {
            Panel.gameObject.SetActive(false);
        }
    }
}
