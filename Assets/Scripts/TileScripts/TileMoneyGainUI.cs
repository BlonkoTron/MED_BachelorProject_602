using UnityEngine;
using TMPro;

public class TileMoneyGainUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyGainText;

    public void SetMoneyGainUI(int money)
    {
        if (money>0)
        {
            moneyGainText.text = "+" + money.ToString();
        } else
        {
            moneyGainText.text = money.ToString();

        }
    }
}
