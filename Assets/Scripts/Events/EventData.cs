using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event Data")]
public class EventData : ScriptableObject
{
    [TextArea] public string eventtitle;
    [TextArea] public string eventInfo;

    public Texture Scenariosprite;

    [System.Serializable]
    public class ChoiceData
    {
        //Text
        public string choiceText;
        public string buttonText;

        //Money/currency
        public int moneyLoss;
        public int moneyGain;
        public int HappinessUp;
        public int HappinessDown;

        //Tile Nerf or Buff
        public float MineEfficiencymult;
        public float CowEfficiencymult;
        public float AggroForestEfficiencymult;
        public float FarmEfficiencymult;

        //TileChanges
        public bool DisableMineTilesEvent;
        public bool DisableCowTilesEvent;
        public bool DisableAgroTilesEvent;
        public bool DisableFarmTilesEvent;

        //Tile degradation buff/nerf
        public float MineDegradation = 1f;
        public float Cowdegradation = 1f;
        public float AggroForestdegradation = 1f;
        public float Farmdegradation = 1f;
    }

    public ChoiceData choice1;
    public ChoiceData choice2;
    public ChoiceData choice3;

    public void choice1_Setup()
    {
        Eventmanager_NEWSETUP.instance.ApplyChoice(choice1);
    }

    public void choice2_Setup()
    {
        Eventmanager_NEWSETUP.instance.ApplyChoice(choice2);
    }

    public void choice3_Setup()
    {
        Eventmanager_NEWSETUP.instance.ApplyChoice(choice3);
    }
}