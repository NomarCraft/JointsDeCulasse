using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangementBouton : MonoBehaviour
{
    public GameObject FirstObjet;
    public void Changment()
    {

        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObjet, null);
    }
}