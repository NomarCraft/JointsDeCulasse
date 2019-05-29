using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchCanevas : MonoBehaviour
{
    public GameObject OffCanevas;
    public GameObject OnCanevas;
    public GameObject FirstObjet;
    public void Switch()
    {
        OffCanevas.SetActive(true);
        OnCanevas.SetActive(false);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(FirstObjet, null);
    }
}
