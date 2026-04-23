using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager instance;

    public List<GameObject> tutorialObjects = new List<GameObject>();

    private int currentStep = 0;

    public UnityEvent nextStep;

    private void Start()
    {
        instance = this;

        currentStep = 0;

        nextStep.AddListener(UpdateTutorial);

        if (tutorialObjects[currentStep] != null)
        {
            tutorialObjects[currentStep].gameObject.SetActive(true);
        }

    }

    private void UpdateTutorial()
    {
        if (tutorialObjects[currentStep] != null)
        {
            tutorialObjects[currentStep].gameObject.SetActive(false);

            currentStep++;
            
            tutorialObjects[currentStep].gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        nextStep.RemoveAllListeners();
    }

}
