using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Event_Manager : MonoBehaviour
{
    // Reference to ScriptableObject
    public static Event_Manager instance { get; private set; }
    public EventData selectedEvent;

    // UI text
    public TMP_Text Eventtitle;
    public TMP_Text Eventinfo;

    public TMP_Text Eventchoice1;
    public TMP_Text Eventchoice2;
    public TMP_Text Eventchoice3;

    public TMP_Text Buttonchoice1;
    public TMP_Text Buttonchoice2;
    public TMP_Text Buttonchoice3;

    public RawImage ScenarioImage;

    // Enable/disable Panel
    public GameObject Panel;

    // Test variables
    public int Testmoney;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Panel.SetActive(false);
    }

    public void Startevent()
    {
        Panel.SetActive(true);

        // Event info
        Eventinfo.text = selectedEvent.eventInfo;
        Eventtitle.text = selectedEvent.eventtitle;

        // Choices text
        Eventchoice1.text = selectedEvent.choice1.choiceText;
        Eventchoice2.text = selectedEvent.choice2.choiceText;
        Eventchoice3.text = selectedEvent.choice3.choiceText;

        // Button text
        Buttonchoice1.text = selectedEvent.choice1.buttonText;
        Buttonchoice2.text = selectedEvent.choice2.buttonText;
        Buttonchoice3.text = selectedEvent.choice3.buttonText;

        // Image
        ScenarioImage.texture = selectedEvent.Scenariosprite;
    }

    public void Option_1()
    {
        selectedEvent.choice1_Setup();
        Panel.SetActive(false);
        Debug.Log("Option 1 picked");
    }

    public void Option_2()
    {
        selectedEvent.choice2_Setup();
        Panel.SetActive(false);
        Debug.Log("Option 2 picked");
    }

    public void Option_3()
    {
        selectedEvent.choice3_Setup();
        Panel.SetActive(false);
        Debug.Log("Option 3 picked");
    }
}