using System.Collections.Generic;
using System.Linq; // 🔥 needed for Except()
using FMODUnity;
using UnityEngine;

public class Eventmanager_NEWSETUP : MonoBehaviour
{
    public static Eventmanager_NEWSETUP instance { get; private set; }

    public List<EventData> allEvents;
    private List<EventData> usedEvents = new List<EventData>(); // 🔥 track used

    public EventData currentEvent;

    private Event_Manager EventMan;

    public TileChanger TileChange;

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
        TileChanger[] allTiles = FindObjectsByType<TileChanger>(FindObjectsSortMode.None);

        //Close UI´s
        foreach (TileChanger tile in allTiles)
        {
            tile.CloseUI();
        }

        if (allEvents.Count == 0) return;

        // get only unused events
        List<EventData> availableEvents = allEvents.Except(usedEvents).ToList();

        // if all events used, reset (optional behavior)
        if (availableEvents.Count == 0)
        {
            usedEvents.Clear();
            availableEvents = new List<EventData>(allEvents);
        }

        // pick random from remaining
        currentEvent = availableEvents[Random.Range(0, availableEvents.Count)];

        usedEvents.Add(currentEvent); // mark as used

        EventMan.selectedEvent = currentEvent;
        EventMan.Startevent();

        Debug.Log(currentEvent.eventInfo);
    }
}