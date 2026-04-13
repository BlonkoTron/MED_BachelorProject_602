using System;
using UnityEngine;
using UnityEngine.UI;

public class Event_Manager : MonoBehaviour
{
    // Reference to ScriptableObject
    public static Event_Manager instance { get; private set; }
    public EventData selectedEvent;
    public bool Spawnevent;

    // UI text
    public Text Eventinfo;

    public Text Eventchoice1;
    public Text Eventchoice2;
    public Text Eventchoice3;

    public Text Buttonchoice1;
    public Text Buttonchoice2;
    public Text Buttonchoice3;

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

        // Choices text
        Eventchoice1.text = selectedEvent.choiceText1;
        Eventchoice2.text = selectedEvent.choiceText2;
        Eventchoice3.text = selectedEvent.choiceText3;

        // Button text
        Buttonchoice1.text = selectedEvent.buttonText1;
        Buttonchoice2.text = selectedEvent.buttonText2;
        Buttonchoice3.text = selectedEvent.buttonText3;

        // Image
        ScenarioImage.texture = selectedEvent.Scenariosprite;

        Spawnevent = false;
    }

    public void Option_1()
    {
        Debug.Log("Option 1 picked");
        Panel.SetActive(false);
        Testmoney -= selectedEvent.moneyLoss;
        Testmoney += selectedEvent.moneyGain;
        selectedEvent = null;
    }

    public void Option_2()
    {
        Debug.Log("Option 2 picked");
        Panel.SetActive(false);
        if (selectedEvent.tileSpawn)
        {

        }
        selectedEvent = null;
    }

    public void Option_3()
    {
        Debug.Log("Option 3 picked");
        Panel.SetActive(false);
        if (selectedEvent.tileDestruction)
        {
        }
        selectedEvent = null;
    }
}