using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : Tutorials
{
    public GameObject Panel;

    public List<string> Keys = new List<string>();


    public override void CheckIfHappeneing()
    {
        Panel.gameObject.SetActive(true);
        Time.timeScale = 0;
        for (int i = 0; i < Keys.Count; i++)
        {          
            if (Input.GetButton("Boost"))
            {
                Panel.gameObject.SetActive(false);
                Time.timeScale = 1;
                Debug.Log("marche");
                Keys.RemoveAt(i);
                break;
                
            }
        }
        if (Keys.Count == 0)
            TutorialManager.Instance.CompletedTutorial();
    }


}
