using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuotaUI : MonoBehaviour
{
    private QuotaManager quotaManager;

    [SerializeField] private TMP_Text quotaAmountText;

    [SerializeField] private TMP_Text timeLeftText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quotaManager = QuotaManager.Instance;

        quotaManager.OnQuotaUpdated.AddListener(UpdateQuotaText);
        UpdateQuotaText();
    }

    private void UpdateQuotaText()
    {
        quotaAmountText.text = quotaManager.CurrentQuotaAmount.ToString();

    }
    private void Update()
    {
        timeLeftText.text = GameManager.Instance.SecondsTillRoundEnd().ToString();
    }
    private void OnDestroy()
    {
        quotaManager.OnQuotaUpdated.RemoveListener(UpdateQuotaText);
    }
}
