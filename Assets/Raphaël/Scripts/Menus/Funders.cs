using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Funders : MonoBehaviour
{
    public Animator ferders;


    void Start()
    {
        ferders = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Start1"))
        {
            ferders.Play("Disparitiontitre");

        }

    }    
}
