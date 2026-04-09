using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Event_Manager : MonoBehaviour
{
    //Eventinfo text
    public Text Eventinfo;

    public Text Eventchoice1;
    public Text Eventchoice2;
    public Text Eventchoice3;

    public Text Buttonchoice1;
    public Text Buttonchoice2;
    public Text Buttonchoice3;

    //Stores eventdata
    private Events selectedEvent;

    public bool Spawnevent;

    //Enable/disable Panel
    public GameObject Panel;

    //List of events
    public List<Events> InGameEvents;

    //Testint
    public int Testmoney;
    public bool HasThirdChoice;
    public GameObject Button3;

    [System.Serializable]
    public class Events
    {
        public string Info;
        public string Choicetext1;
        public string Choicetext2;
        public string Choicetext3;
        public string Buttontext1;
        public string Buttontext2;
        public string Buttontext3;
        public int Moneyloss;
        public int Moneygain;
        public bool Tiledestuction;
        public bool TileSpawn;

    }

    private void Start()
    {
        Panel.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Spawnevent)
        {
            Panel.SetActive(true);
            selectedEvent = InGameEvents[UnityEngine.Random.Range(0, InGameEvents.Count)];

            Eventinfo.text = selectedEvent.Info;

            // Event text
            Eventchoice1.text = selectedEvent.Choicetext1;
            Eventchoice2.text = selectedEvent.Choicetext2;

            // Button text
            Buttonchoice1.text = selectedEvent.Buttontext1;
            Buttonchoice2.text = selectedEvent.Buttontext2;

            // Third choice check
            if (HasThirdChoice == true)
            {
                Eventchoice3.gameObject.SetActive(true);
                Button3.gameObject.SetActive(true);

                Eventchoice3.text = selectedEvent.Choicetext3;
                Buttonchoice3.text = selectedEvent.Buttontext3;
            }
            else
            {
                Eventchoice3.gameObject.SetActive(false);
                Button3.gameObject.SetActive(false);
            }

            Spawnevent = false;
        }
    }

    public void Buttonok()
    {
        //Remove money
        Testmoney = Testmoney - selectedEvent.Moneyloss;
        //Add money
        Testmoney = Testmoney + selectedEvent.Moneygain;

        if (selectedEvent.Tiledestuction)
        {
            Debug.Log("Destorytile");
            //Destroy/replace tile here
        }

        if (selectedEvent.TileSpawn)
        {
            Debug.Log("Spawntile");
            //Spawn tile here
        }

        //Remove panel
        Panel.SetActive(false);
    }

    public void Option_1()
    {
        Debug.Log("Option 1 picked");
        Panel.SetActive(false);
    }

    public void Option_2()
    {
        Debug.Log("Option 2 picked");
        Panel.SetActive(false);
    }

    public void Option_3()
    {
        Debug.Log("Option 3 picked");
        Panel.SetActive(false);
    }
}
