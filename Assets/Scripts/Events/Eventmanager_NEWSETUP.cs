using System.Collections.Generic;
using System.Linq; // 🔥 needed for Except()
using FMODUnity;
using UnityEngine;
using static EventData;

public class Eventmanager_NEWSETUP : MonoBehaviour
{
    public static Eventmanager_NEWSETUP instance { get; private set; }

    public List<EventData> allEvents;
    private List<EventData> usedEvents = new List<EventData>(); // 🔥 track used

    public Tile tilescript;

    public EventData currentEvent;

    private Event_Manager EventMan;

    public TileChanger TileChange;

    public bool Test_Triggerevent = false;

    //Degredationmults
    public float MineDEGRADATION_Mult = 1f;
    public float CowDEGRADATION_Mult = 1f;
    public float FarmDEGRADATION_Mult = 1f;
    public float AgroDEGRADATION_Mult = 1f;

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

    public void ApplyChoice(ChoiceData choice)
    {
        Debug.Log("ApplyChoice called");

        //Money calls
        int ED_currentmoney = PointSystem.Instance.CurrentMoney;
        if (choice.moneyGain!=0)
        {
            PointSystem.Instance.AddMoney(choice.moneyGain);
        }
        if (choice.moneyLoss!=0)
        {
            PointSystem.Instance.LoseMoney(choice.moneyLoss);
        }

        if (PointSystem.Instance.CurrentMoney > ED_currentmoney)
        {
            // plus money
        }
        else if (PointSystem.Instance.CurrentMoney < ED_currentmoney)
        {
            // minus money
        }
        else
        {
            // no change
        }

        //Efficientmultiply calls
        PointSystem.Instance.mineEfficiencyMultiplier = choice.MineEfficiencymult;
        PointSystem.Instance.cowfieldEfficiencyMultiplier = choice.CowEfficiencymult;
        PointSystem.Instance.agroforestEfficiencyMultiplier = choice.AggroForestEfficiencymult;
        PointSystem.Instance.farmEfficiencyMultiplier = choice.FarmEfficiencymult;
        PointSystem.Instance.UpdateEfficiencyUI();

        //Happiness calls
        Happiness.Instance.DecreaseHappiness(choice.HappinessDown);
        Happiness.Instance.IncreaseHappiness(choice.HappinessUp);

        //Start round
        GameManager.Instance.StartRound();

        //Disable tile
        GameManager.Instance.DisableMineTilesGamesetting = choice.DisableMineTilesEvent;
        GameManager.Instance.DisableFarmTilesGamesetting = choice.DisableFarmTilesEvent;
        GameManager.Instance.DisableAgroTilesGamesetting = choice.DisableAgroTilesEvent;
        GameManager.Instance.DisableCowTilesGamesetting = choice.DisableCowTilesEvent;

        //DEGRADATION mult change
        MineDEGRADATION_Mult = choice.MineDegradation;
        FarmDEGRADATION_Mult = choice.Farmdegradation;
        AgroDEGRADATION_Mult = choice.AggroForestdegradation;
        CowDEGRADATION_Mult = choice.Cowdegradation;
    }
}