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

    [SerializeField] private Animator eventNoticeAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quotaManager = QuotaManager.Instance;
        gameManager = GameManager.Instance;

        quotaManager.OnQuotaUpdated.AddListener(UpdateQuotaText);
        gameManager.onGameTick.AddListener(UpdateEventNotice);
        gameManager.onRoundStart.AddListener(UpdateEventNotice);
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
    private void UpdateEventNotice()
    {
        if (gameManager.TicksTillRoundEnd<=2)
        {
            // last tick anim
            eventNoticeAnimator.SetBool("lastTick", true);
        } else
        {
            eventNoticeAnimator.SetBool("lastTick", false);
        }
    }
}
