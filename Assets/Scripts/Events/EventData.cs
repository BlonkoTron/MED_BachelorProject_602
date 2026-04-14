using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event Data")]
public class EventData : ScriptableObject
{
    [TextArea] public string eventInfo;

    public Texture Scenariosprite;

    [System.Serializable]
    public class ChoiceData
    {
        public string choiceText;
        public string buttonText;

        public int moneyLoss;
        public int moneyGain;
        public bool tileDestruction;
        public bool tileSpawn;
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

        if (choice.tileDestruction)
        {
            Debug.Log("Destroy tile");
        }

        if (choice.tileSpawn)
        {
            Debug.Log("Spawn tile");
        }
    }
}