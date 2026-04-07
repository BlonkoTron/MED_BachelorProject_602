using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

public class Event_Manager : MonoBehaviour
{
    //Eventinfo text
    public Text Eventinfo;

    //Stores eventdata
    private Events selectedEvent;

    public bool Spawnevent;

    //Enable/disable Panel
    public GameObject Panel;

    //List of events
    public List<Events> InGameEvents;

    //Testint
    public int Testmoney;

    [System.Serializable]
    public class Events
    {
        public string Info;
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
}
