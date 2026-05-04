using UnityEngine;
using TMPro;

public class HappinessGainUI : MonoBehaviour
{
    [SerializeField] private TMP_Text happinessGainText;

    public void SetHappinessGainUI(int happinessChange)
    {
        if (happinessChange > 0)
        {
            happinessGainText.text = "+" + happinessChange.ToString();
        }
        else
        {
            happinessGainText.text = happinessChange.ToString();

        }
    }
}
