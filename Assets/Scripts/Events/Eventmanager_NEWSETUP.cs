using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Eventmanager_NEWSETUP : MonoBehaviour
{
    public static Eventmanager_NEWSETUP instance { get; private set; }

    public List<EventData> allEvents;

    public EventData currentEvent;

    [SerializeField] private Event_Manager EventMan;

    public bool Test_Triggerevent = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EventMan = Event_Manager.instance;
    }

    private void Update()
    {
        if (Test_Triggerevent == true)
        {
            TriggerRandomEvent();
            Test_Triggerevent = false;
        }
    }

    public void TriggerRandomEvent()
    {
        if (allEvents.Count == 0) return;

        currentEvent = allEvents[Random.Range(0, allEvents.Count)];
        EventMan.selectedEvent = currentEvent;
        EventMan.Startevent();

        Debug.Log(currentEvent.eventInfo);
    }
}