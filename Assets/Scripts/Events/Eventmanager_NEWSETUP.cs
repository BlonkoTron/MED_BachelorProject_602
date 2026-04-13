using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Eventmanager_NEWSETUP : MonoBehaviour
{
    public List<EventData> allEvents;

    private EventData currentEvent;

    [SerializeField] private Event_Manager EventMan;

    public bool test;

    private void Start()
    {
        EventMan = Event_Manager.instance;
    }

    private void Update()
    {
        if (test == true)
        {
            TriggerRandomEvent();
            test = false;
        }
    }

    public void TriggerRandomEvent()
    {
        if (allEvents.Count == 0) return;

        currentEvent = allEvents[Random.Range(0, allEvents.Count)];
        EventMan.selectedEvent = currentEvent;

        Debug.Log(currentEvent.eventInfo);
    }
}