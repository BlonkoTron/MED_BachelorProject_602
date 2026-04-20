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
        public bool tileDestruction;
        public bool tileSpawnMine;
        public bool tileSpawnFarm;
    }

    public ChoiceData choice1;
    public ChoiceData choice2;
    public ChoiceData choice3;

    public void choice1_Setup()
    {
        ApplyChoice(choice1);
    }

    public void choice2_Setup()
    {
        ApplyChoice(choice2);
    }

    public void choice3_Setup()
    {
        ApplyChoice(choice3);
    }

    private void ApplyChoice(ChoiceData choice)
    {
        Debug.Log("ApplyChoice called");
        PointSystem.Instance.AddMoney(choice.moneyGain);
        PointSystem.Instance.LoseMoney(choice.moneyLoss);
        PointSystem.Instance.mineEfficiencyMultiplier = choice.MineEfficiencymult;
        PointSystem.Instance.cowfieldEfficiencyMultiplier = choice.CowEfficiencymult;
        PointSystem.Instance.agroforestEfficiencyMultiplier = choice.AggroForestEfficiencymult;
        PointSystem.Instance.farmEfficiencyMultiplier = choice.FarmEfficiencymult;
        Happiness.Instance.DecreaseHappiness(choice.HappinessDown);
        Happiness.Instance.IncreaseHappiness(choice.HappinessUp);
    }
}