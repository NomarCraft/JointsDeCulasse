using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : Tutorials
{

    public List<string> Keys = new List<string>();


    public override void CheckIfHappeneing()
    {
        Time.timeScale = 0;
        for (int i = 0; i < Keys.Count; i++)
        {          
            if (Input.inputString.Contains(Keys[i]))
            {
                Debug.Log("marche");
                Time.timeScale = 1;
                Keys.RemoveAt(i);
                break;
                
            }
        }
        if (Keys.Count == 0)
            TutorialManager.Instance.CompletedTutorial();
    }
}
