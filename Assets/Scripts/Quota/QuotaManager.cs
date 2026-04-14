using UnityEngine;
using UnityEngine.Events;

public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance;

    private int currentQuotaAmount;
    private int currentQuotaIndex = 0;

    public int CurrentQuotaAmount => currentQuotaAmount;

    [SerializeField] private int[] quotaAmounts;

    private GameManager gameManager;
    private PointSystem pointSystem;

    public UnityEvent OnQuotaUpdated;
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
        currentQuotaAmount = quotaAmounts[0];
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        pointSystem = PointSystem.Instance;
        if (gameManager!=null)
        {
            gameManager.onRoundEnd.AddListener(OnGameManagerRoundEnd);
        }
    }
    private void OnGameManagerRoundEnd()
    {
        // pay the quota money
        if (pointSystem != null)
        {
            pointSystem.SpendMoney(currentQuotaAmount);
        }

        // update to new quota
        currentQuotaAmount = NewQuota();
        OnQuotaUpdated.Invoke();
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
    private void OnDestroy()
    {
        gameManager.onGameTick.RemoveListener(OnGameManagerRoundEnd);

    }
}
