using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuotaUI : MonoBehaviour
{
    private QuotaManager quotaManager;
    private GameManager gameManager;

    [SerializeField] private TMP_Text quotaAmountText;

    [SerializeField] private TMP_Text timeLeftText;

    [SerializeField] private Image roundProgressBar;

    [SerializeField] private Animator clockAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quotaManager = QuotaManager.Instance;
        gameManager = GameManager.Instance;

        quotaManager.OnQuotaUpdated.AddListener(UpdateQuotaText);
        UpdateQuotaText();
    }

    private void UpdateQuotaText()
    {
        quotaAmountText.text = quotaManager.CurrentQuotaAmount.ToString();

    }
    private void Update()
    {
        timeLeftText.text = gameManager.SecondsTillRoundEnd().ToString();
        roundProgressBar.fillAmount = gameManager.roundProgressValue;
    }
    private void OnDestroy()
    {
        quotaManager.OnQuotaUpdated.RemoveListener(UpdateQuotaText);
    }
}
