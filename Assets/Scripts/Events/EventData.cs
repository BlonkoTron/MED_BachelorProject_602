using UnityEngine;

[CreateAssetMenu(menuName = "Events/Event Data")]
public class EventData : ScriptableObject
{
    [TextArea] public string eventInfo;

    public string choiceText1;
    public string choiceText2;
    public string choiceText3;

    public string buttonText1;
    public string buttonText2;
    public string buttonText3;

    public int moneyLoss;
    public int moneyGain;

    public bool tileDestruction;
    public bool tileSpawn;
}