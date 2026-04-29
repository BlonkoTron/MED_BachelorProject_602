using TMPro;
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

    [SerializeField] private GameObject farm2xUI;
    [SerializeField] private GameObject farm05xUI;
    [SerializeField] TextMeshProUGUI farmpositive;
    [SerializeField] TextMeshProUGUI farmNegative;

    [SerializeField] private GameObject mine2xUI;
    [SerializeField] private GameObject mine05xUI;
    [SerializeField] TextMeshProUGUI minepositive;
    [SerializeField] TextMeshProUGUI mineNegative;

    [SerializeField] private GameObject cow2xUI;
    [SerializeField] private GameObject cow05xUI;
    [SerializeField] TextMeshProUGUI cowpositive;
    [SerializeField] TextMeshProUGUI cowNegative;

    [SerializeField] private GameObject agro2xUI;
    [SerializeField] private GameObject agro05xUI;
    [SerializeField] TextMeshProUGUI agropositive;
    [SerializeField] TextMeshProUGUI agroNegative;

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
        onMoneyEarned.Invoke(currentMoney);
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

    public void UpdateEfficiencyUI()
    {
        UpdateSingleUI(farmEfficiencyMultiplier, farmpositive, farmNegative);
        UpdateSingleUI(mineEfficiencyMultiplier, minepositive, mineNegative);
        UpdateSingleUI(cowfieldEfficiencyMultiplier, cowpositive, cowNegative);
        UpdateSingleUI(agroforestEfficiencyMultiplier, agropositive, agroNegative);

        UpdateUIForType(TileType.Farm, farmEfficiencyMultiplier, farm2xUI, farm05xUI);
        UpdateUIForType(TileType.Mine, mineEfficiencyMultiplier, mine2xUI, mine05xUI);
        UpdateUIForType(TileType.CowField, cowfieldEfficiencyMultiplier, cow2xUI, cow05xUI);
        UpdateUIForType(TileType.Agroforest, agroforestEfficiencyMultiplier, agro2xUI, agro05xUI);
    }

    private void UpdateUIForType(TileType type, float multiplier, GameObject goodUI, GameObject badUI)
    {
        bool isGood = multiplier > 1f;
        bool isBad = multiplier < 1f;

        goodUI.SetActive(isGood);
        badUI.SetActive(isBad);
    }

    void UpdateSingleUI(float multiplier, TextMeshProUGUI positiveText, TextMeshProUGUI negativeText)
    {
        string value = multiplier.ToString("0.00") + "x";

        if (multiplier > 1f)
        {
            positiveText.gameObject.SetActive(true);
            negativeText.gameObject.SetActive(false);

            positiveText.text = value;
        }
        else if (multiplier < 1f)
        {
            positiveText.gameObject.SetActive(false);
            negativeText.gameObject.SetActive(true);

            negativeText.text = value;
        }
        else
        {
            // exactly 1 → optional: hide both or pick one
            positiveText.gameObject.SetActive(false);
            negativeText.gameObject.SetActive(false);
        }
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
