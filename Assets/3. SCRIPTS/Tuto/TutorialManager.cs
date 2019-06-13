using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    public List<Tutorials> Tutorials = new List<Tutorials>();

    public TextMeshProUGUI expText;


    private static TutorialManager instance;

    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<TutorialManager>();

            if (instance == null)
                Debug.Log("There is no TutorialManager");

            return instance; 
        }
    }

    private Tutorials currentTutorial;

    
    void Start()
    {
        SetNextTutorial(0);
    }


    void Update()
    {
        if (currentTutorial)
            currentTutorial.CheckIfHappeneing();


    }
    public void CompletedTutorial()
    {
        SetNextTutorial(currentTutorial.Order + 1);
        
        
    }


    public void SetNextTutorial(int currentOrder)
    {
       
        currentTutorial = GetTutorialsByOrder(currentOrder);

        if(!currentTutorial)
        {
            CompletedAllTutorials();
            return;
        }
        expText.text = currentTutorial.Explanation;
    }

    public void CompletedAllTutorials()
    {
        expText.text = "Explique le boost scripts TutoManager";

        // Loadnextscene
    }



    public Tutorials GetTutorialsByOrder(int Order)
    {
        for (int i = 0; i < Tutorials.Count; i++)
        {
            if (Tutorials[i].Order == Order)
                return Tutorials[i];
        }

        return null;
    }

}
