using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarEngine>())
        {
            other.gameObject.GetComponentInParent<CarEngine>().BumperContact();
        }
        
    }
}
