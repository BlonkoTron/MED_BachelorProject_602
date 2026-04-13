using UnityEngine;
using UnityEngine.Events;

public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance;

    [SerializeField] private int ticksBetweenQuotas = 5;
    private int ticksTillNextQuota;
    private int currentQuotaAmount;
    private int currentQuotaIndex = 0;

    public int CurrentQuotaAmount => currentQuotaAmount;
    public int TicksTillNextQuota => ticksTillNextQuota;

    [SerializeField] private int[] quotaAmounts;

    private GameManager gameManager;
    private PointSystem pointSystem;

    public UnityEvent OnQuotaUpdated;
    public UnityEvent OnQuotaPaymentTime;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        // setup for first quota
        ticksTillNextQuota = ticksBetweenQuotas;
        currentQuotaAmount = quotaAmounts[0];
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        pointSystem = PointSystem.Instance;
        // subscribe to game manager tick
        if (gameManager!=null)
        {
            gameManager.onGameTick.AddListener(OnGameManagerTick);
        }
    }

    private int NewQuota()
    {
        currentQuotaIndex++;
        // update quota if higher index exists. Else use the last value in the array
        if (quotaAmounts.Length>currentQuotaIndex)
        {
            return quotaAmounts[currentQuotaIndex];
        } else
        {
            return quotaAmounts[quotaAmounts.Length-1];
        }
    }

    private void OnGameManagerTick()
    {
        ticksTillNextQuota--;
        if (ticksTillNextQuota<=0)
        {
            OnQuotaPaymentTime.Invoke();
            // pay the quota money
            if (pointSystem!=null)
            {
                pointSystem.SpendMoney(currentQuotaAmount);
            }

            // update to new quota
            currentQuotaAmount = NewQuota();
            ticksTillNextQuota = ticksBetweenQuotas;
            OnQuotaUpdated.Invoke();
        }
    }
    private void OnDestroy()
    {
        gameManager.onGameTick.RemoveListener(OnGameManagerTick);

    }
}
