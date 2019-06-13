using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : Tutorials
{


    private bool isCurrentTutorial = false;

    public Transform HitTransform;

    public bool active;

    public override void CheckIfHappeneing()
    {

        isCurrentTutorial = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!isCurrentTutorial)
            return;
        if (other.transform == HitTransform)
        {

            TutorialManager.Instance.CompletedTutorial();
            isCurrentTutorial = false;
   
        }
        else if (other.GetComponentInParent<Transform>() == HitTransform )
        {
            TutorialManager.Instance.CompletedTutorial();
            isCurrentTutorial = false;
        }
    }



}
