using UnityEngine;
using UnityEngine.Events;

public class PointSystem : MonoBehaviour
{
    public static PointSystem Instance;
    [Header("Money Tracking")]
    [SerializeField] private int currentMoney = 0;

    [SerializeField] private Eventmanager_NEWSETUP Eventsir;
    
    public int CurrentMoney => currentMoney;

    [HideInInspector] public float farmEfficiencyMultiplier = 1;
    [HideInInspector] public float mineEfficiencyMultiplier = 1;
    [HideInInspector] public float cowfieldEfficiencyMultiplier = 1;
    [HideInInspector] public float agroforestEfficiencyMultiplier = 1;


    [HideInInspector] public UnityEvent<int> onMoneyEarned;
    [HideInInspector] public UnityEvent<int> onMoneySpent;
    [HideInInspector] public UnityEvent<int> onMoneyLost;

    [SerializeField] private GameObject efficiencyUI;

    private GameSettings gameSettings;

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
    }
    void Start()
    {
        //Get Eventmanager_NEWSETUP 
        Eventsir = Eventmanager_NEWSETUP.instance;

        gameSettings = GameManager.Instance.gameSettings;

        currentMoney = gameSettings.STARTING_MONEY;
        // Find all tiles and subscribe to their money events
        RegisterAllTiles();
    }
    
    private void RegisterAllTiles()
    {
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        foreach (Tile tile in allTiles)
        {
            // Remove listener first to prevent duplicates
            tile.onMoneyEarned.RemoveListener(AddMoney);
            tile.onMoneyEarned.AddListener(AddMoney);
        }
        
        Debug.Log($"PointSystem registered {allTiles.Length} tiles for money tracking");
    }
    
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        onMoneyEarned.Invoke(amount);
        Debug.Log($"Earned ${amount}. Total money: ${currentMoney}");
    }
    public void LoseMoney(int amount)
    {
        currentMoney -= amount;
        onMoneyLost.Invoke(amount);
        Debug.Log($"lost ${amount}. Total money: ${currentMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            onMoneySpent.Invoke(amount);
            Debug.Log($"Spent ${amount}. Remaining money: ${currentMoney}");
            return true;
        }
        else
        {
            Debug.Log($"Not enough money! Need ${amount}, have ${currentMoney}");
            return false;
        }
    }
    public float GetEfficiencyMultiplier(TileType type)
    {
        switch (type)
        {
            case TileType.Farm:
                return farmEfficiencyMultiplier;
            case TileType.CowField:
                return cowfieldEfficiencyMultiplier;
            case TileType.Mine:
                return mineEfficiencyMultiplier;
            case TileType.Agroforest:
                return agroforestEfficiencyMultiplier;
            default:
                return 1;
        }
    }

    public void SetEfficiencyMultiplier(TileType type, float value)
    {
        switch (type)
        {
            case TileType.Farm:
                farmEfficiencyMultiplier = value;
                break;
            case TileType.CowField:
                cowfieldEfficiencyMultiplier = value;
                break;
            case TileType.Mine:
                mineEfficiencyMultiplier = value;
                break;
            case TileType.Agroforest:
                agroforestEfficiencyMultiplier = value;
                break;
        }
        UpdateEfficiencyUI();
    }

    private void UpdateEfficiencyUI()
    {
        bool shouldShow =
            farmEfficiencyMultiplier == 0.5f || farmEfficiencyMultiplier == 2f ||
            mineEfficiencyMultiplier == 0.5f || mineEfficiencyMultiplier == 2f ||
            cowfieldEfficiencyMultiplier == 0.5f || cowfieldEfficiencyMultiplier == 2f ||
            agroforestEfficiencyMultiplier == 0.5f || agroforestEfficiencyMultiplier == 2f;

        efficiencyUI.SetActive(shouldShow);
    }

    void OnDestroy()
    {
        // Unsubscribe from all tiles
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        foreach (Tile tile in allTiles)
        {
            tile.onMoneyEarned.RemoveListener(AddMoney);
        }
    }
}
