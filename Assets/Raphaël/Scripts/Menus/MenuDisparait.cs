using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuDisparait : MonoBehaviour
{
    public GameObject pressStart;
    public bool active;
    public GameObject firstObjet;

    public GameObject playButton;
    public GameObject inputButton;
    public GameObject settingButton;
    public GameObject creditButton;
    public GameObject quitButton;
    public GameObject fenders;

    void Update()
    {
        if (Input.GetButton("Start1"))
        {
            if (active == true)
            {

                pressStart.SetActive(true);
                playButton.SetActive(false);
                inputButton.SetActive(false);
                settingButton.SetActive(false);
                creditButton.SetActive(false);
                quitButton.SetActive(false);
                fenders.SetActive(false);


            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(firstObjet, null);
                pressStart.SetActive(false);
                playButton.SetActive(true);
                inputButton.SetActive(true);
                settingButton.SetActive(true);
                creditButton.SetActive(true);
                quitButton.SetActive(true);
                fenders.SetActive(true);

            }
        }

    }
}
