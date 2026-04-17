using UnityEngine;
using UnityEngine.Events;

public class Happiness : MonoBehaviour
{
    public static Happiness Instance;

    [Header("Happiness Settings")]
    [SerializeField] private int happinessLevel = 100;
    [SerializeField] private int maxHappiness = 100;
    [SerializeField] private int minHappiness = 0;

    [Header("Happiness Thresholds")]
    [SerializeField] private int criticalThreshold = 20;
    [SerializeField] private int lowThreshold = 40;
    [SerializeField] private int goodThreshold = 70;

    [Header("Periodic Happiness Changes (per game tick)")]
    [Tooltip("Barren percentage above which happiness decreases by a large amount")]
    [SerializeField][Range(0f, 1f)] private float barrenHighThreshold = 0.3f;
    [Tooltip("Happiness change when barren tiles exceed high threshold")]
    [SerializeField] private int barrenHighPenalty = 4;
    
    [Tooltip("Barren percentage above which happiness decreases by a small amount")]
    [SerializeField][Range(0f, 1f)] private float barrenLowThreshold = 0.1f;
    [Tooltip("Happiness change when barren tiles exceed low threshold")]
    [SerializeField] private int barrenLowPenalty = 2;
    
    [Tooltip("Natural tile percentage above which happiness increases by a large amount")]
    [SerializeField][Range(0f, 1f)] private float naturalHighThreshold = 0.5f;
    [Tooltip("Happiness change when natural tiles exceed high threshold")]
    [SerializeField] private int naturalHighBonus = 4;
    
    [Tooltip("Natural tile percentage above which happiness increases by a small amount")]
    [SerializeField][Range(0f, 1f)] private float naturalLowThreshold = 0.3f;
    [Tooltip("Happiness change when natural tiles exceed low threshold")]
    [SerializeField] private int naturalLowBonus = 2;

    [Header("Events")]
    [HideInInspector] public UnityEvent<int> onHappinessChanged;
    [HideInInspector] public UnityEvent<int> onHappinessIncreased;
    [HideInInspector] public UnityEvent<int> onHappinessDecreased;
    [HideInInspector] public UnityEvent onHappinessCritical;

    // Public property to access current happiness level
    public int HappinessLevel => happinessLevel;

    private void Awake()
    {
        // Singleton pattern
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
        Debug.Log($"Happiness System initialized at {happinessLevel}%");
        
        // Subscribe to game tick events for periodic happiness changes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameTick.AddListener(OnGameTick);
            Debug.Log("Happiness system subscribed to game ticks");
        }
        else
        {
            Debug.LogWarning("GameManager not found - happiness won't update with game ticks");
        }
    }

    private void OnGameTick()
    {
        // Calculate periodic happiness change based on tile composition
        CalculatePeriodicHappinessChange();
    }

    private void CalculatePeriodicHappinessChange()
    {
        // Find all tiles
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        if (allTiles.Length == 0) return;

        // Count tiles by type
        int naturalTiles = 0;      // Rainforest
        int barrenTiles = 0;

        foreach (Tile tile in allTiles)
        {
            switch (tile.Type)
            {
                case TileType.Mine:
                case TileType.CowField:
                case TileType.Agroforest:
                case TileType.Farm:
                case TileType.Grass:
                    break;
                case TileType.Rainforest:
                    naturalTiles++;
                    break;
                case TileType.Barren:
                    barrenTiles++;
                    break;
            }
        }

        int totalTiles = allTiles.Length;
        
        // Calculate percentages
        float naturalPercent = (float)naturalTiles / totalTiles;
        float barrenPercent = (float)barrenTiles / totalTiles;

        // Determine happiness change per tick
        int happinessChange = 0;

        // Barren land always decreases happiness
        if (barrenPercent > barrenHighThreshold)
        {
            happinessChange -= barrenHighPenalty;
        }
        else if (barrenPercent > barrenLowThreshold)
        {
            happinessChange -= barrenLowPenalty;
        }

        // Natural tiles increase happiness
        if (naturalPercent > naturalHighThreshold)
        {
            happinessChange += naturalHighBonus;
        }
        else if (naturalPercent > naturalLowThreshold)
        {
            happinessChange += naturalLowBonus;
        }

        // Apply the change
        if (happinessChange > 0)
        {
            IncreaseHappiness(happinessChange);
            Debug.Log($"Periodic happiness increase: +{happinessChange} (Natural: {naturalPercent:P0}, Barren: {barrenPercent:P0})");
        }
        else if (happinessChange < 0)
        {
            DecreaseHappiness(-happinessChange);
            Debug.Log($"Periodic happiness decrease: {happinessChange} (Natural: {naturalPercent:P0},  Barren: {barrenPercent:P0})");
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from game tick events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameTick.RemoveListener(OnGameTick);
        }
    }

    /// <summary>
    /// Decreases happiness by the specified amount
    /// </summary>
    public void DecreaseHappiness(int amount)
    {
        if (amount <= 0) return;

        int previousHappiness = happinessLevel;
        happinessLevel -= amount;
        
        if (happinessLevel < minHappiness)
        {
            happinessLevel = minHappiness;
        }

        int actualChange = previousHappiness - happinessLevel;
        
        if (actualChange > 0)
        {
            Debug.Log($"Happiness decreased by {actualChange}. Current: {happinessLevel}%");
            onHappinessDecreased?.Invoke(actualChange);
            onHappinessChanged?.Invoke(happinessLevel);

            // Check for critical happiness
            if (happinessLevel <= criticalThreshold)
            {
                onHappinessCritical?.Invoke();
                Debug.LogWarning($"Happiness is CRITICAL: {happinessLevel}%");
            }
        }
    }

    /// <summary>
    /// Increases happiness by the specified amount
    /// </summary>
    public void IncreaseHappiness(int amount)
    {
        if (amount <= 0) return;

        int previousHappiness = happinessLevel;
        happinessLevel += amount;
        
        if (happinessLevel > maxHappiness)
        {
            happinessLevel = maxHappiness;
        }

        int actualChange = happinessLevel - previousHappiness;
        
        if (actualChange > 0)
        {
            Debug.Log($"Happiness increased by {actualChange}. Current: {happinessLevel}%");
            onHappinessIncreased?.Invoke(actualChange);
            onHappinessChanged?.Invoke(happinessLevel);
        }
    }

    /// <summary>
    /// Sets happiness to a specific value
    /// </summary>
    public void SetHappiness(int value)
    {
        int previousHappiness = happinessLevel;
        happinessLevel = Mathf.Clamp(value, minHappiness, maxHappiness);

        if (happinessLevel != previousHappiness)
        {
            Debug.Log($"Happiness set to {happinessLevel}%");
            onHappinessChanged?.Invoke(happinessLevel);

            if (happinessLevel <= criticalThreshold)
            {
                onHappinessCritical?.Invoke();
            }
        }
    }

    /// <summary>
    /// Returns the current happiness state
    /// </summary>
    public string GetHappinessState()
    {
        if (happinessLevel <= criticalThreshold)
            return "CRITICAL";
        else if (happinessLevel <= lowThreshold)
            return "LOW";
        else if (happinessLevel <= goodThreshold)
            return "GOOD";
        else
            return "EXCELLENT";
    }
}
