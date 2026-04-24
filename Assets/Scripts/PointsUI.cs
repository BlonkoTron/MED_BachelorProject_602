using UnityEngine;
using TMPro;

public class PointsUI : MonoBehaviour
{
    private PointSystem pointSystem;

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text moneyPerDayText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pointSystem = PointSystem.Instance;
        pointSystem.onMoneyEarned.AddListener(OnMoneyAdded);
        pointSystem.onMoneySpent.AddListener(OnMoneyRemoved);
        pointSystem.onMoneyLost.AddListener(OnMoneyRemoved);
        OnMoneyAdded(0);
    }

    private void OnMoneyAdded(int money)
    {
        moneyText.text = pointSystem.CurrentMoney.ToString();
        moneyPerDayText.text ="+"+ TileTypeAndAmountUI.Instance.GetTotalMoneyGain().ToString() +"/dag";
    }
    private void OnMoneyRemoved(int money)
    {
        moneyText.text = pointSystem.CurrentMoney.ToString();
        moneyPerDayText.text = "+" + TileTypeAndAmountUI.Instance.GetTotalMoneyGain().ToString() + "/dag";
    }

}
