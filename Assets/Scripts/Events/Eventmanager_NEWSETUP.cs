    using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using static EventData;

public class Eventmanager_NEWSETUP : MonoBehaviour
{
    public static Eventmanager_NEWSETUP instance { get; private set; }

    public List<EventData> allEvents;

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

        foreach (TileChanger tile in allTiles)
        {
            tile.CloseUI();
        }

        if (allEvents.Count == 0) return;

        currentEvent = allEvents[Random.Range(0, allEvents.Count)];
        EventMan.selectedEvent = currentEvent;
        EventMan.Startevent();

        Debug.Log(currentEvent.eventInfo);
    }

    public void ApplyChoice(ChoiceData choice)
    {
        Debug.Log("ApplyChoice called");

        //Money calls
        int ED_currentmoney = PointSystem.Instance.CurrentMoney;
        PointSystem.Instance.AddMoney(choice.moneyGain);
        PointSystem.Instance.LoseMoney(choice.moneyLoss);

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
    }
}