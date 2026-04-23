using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager instance;

    public List<GameObject> tutorialObjects = new List<GameObject>();

    private int currentStep = 0;

    public UnityEvent nextStep;

    public bool hasFirstBuilded;
    public bool hasFirstClicked;

    private void Start()
    {
        instance = this;

        currentStep = 0;

        nextStep.AddListener(UpdateTutorial);

        InteractionManager.Instance.EnableDisablePlayerInput();

        if (tutorialObjects[currentStep] != null)
        {
            tutorialObjects[currentStep].gameObject.SetActive(true);
        }

        hasFirstBuilded = false;

        GameManager.Instance.SetNewState(GameManager.GameState.Paused);

    }

    public void UpdateTutorial()
    {
        if (tutorialObjects[currentStep] != null)
        {
            tutorialObjects[currentStep].gameObject.SetActive(false);

            if (currentStep +1 < tutorialObjects.Count)
            {
                currentStep++;
                tutorialObjects[currentStep].gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Tutorial Is now Done");
                Destroy(gameObject);
            }

            Debug.Log(currentStep);

        }
    }

    

    private void OnDestroy()
    {
        InteractionManager.Instance.EnableDisablePlayerInput();
        GameManager.Instance.SetNewState(GameManager.GameState.normal);
        nextStep.RemoveAllListeners();
    }

}
