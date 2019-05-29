using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarEngine>())
        {
            other.gameObject.GetComponentInParent<CarEngine>().Slower();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarEngine>())
        {
            other.gameObject.GetComponentInParent<CarEngine>().UnSlower();
        }
    }
}
