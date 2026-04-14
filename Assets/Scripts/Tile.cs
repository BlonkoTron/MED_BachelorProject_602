using UnityEngine;
using UnityEngine.Events;

public enum TileType
{
    Grass,
    Rainforest,
    Farm,
    CowField,
    Agroforest,
    Barren,
    Mine
}

public class Tile : MonoBehaviour
{
    [Header("Tile Configuration")]
    [SerializeField] private TileType tileType = TileType.Grass;
    
    [Header("Degradation")]
    [SerializeField] private float currentDegradation = 0f;
    [SerializeField] private float degradationThreshold = 100f;
    
    [Header("Materials")]
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material degradedMaterial;
    [SerializeField] private Material criticalMaterial;
    [SerializeField] private Material barrenMaterial;
    
    [Header("Barren Prefab")]
    [SerializeField] private GameObject barrenPrefab;

    [SerializeField] private float placementHeight = 0.1f; // For grass/rainforest natural regeneration
    
    [Header("Material Thresholds (% of max degradation)")]
    [Range(0f, 1f)]
    [SerializeField] private float degradedThreshold = 0.33f; // 33% degraded
    [Range(0f, 1f)]
    [SerializeField] private float criticalThreshold = 0.66f; // 66% degraded
    
    // Event that broadcasts when money is earned
    [HideInInspector] public UnityEvent<int> onMoneyEarned = new UnityEvent<int>();
    
    private Renderer tileRenderer;
    
    // Money Per Tick Constants
    public const int MONEY_MINE = 100;
    public const int MONEY_COW_FIELD = 60;
    public const int MONEY_FARM = 50;
    public const int MONEY_AGROFOREST = 30;
    public const int MONEY_NATURAL = 0;
    
    // Degradation Rate Constants
    public const float DEGRADATION_MINE = 50f;
    public const float DEGRADATION_COW_FIELD = 34f;
    public const float DEGRADATION_FARM = 25f;
    public const float DEGRADATION_AGROFOREST = 10f;
    public const float DEGRADATION_NATURAL_REGEN = -10f;
    public const float DEGRADATION_NONE = 0f;

    // Tile Properties
    public TileType Type => tileType;
    public float CurrentDegradation => currentDegradation;
    public bool IsBarren => tileType == TileType.Barren;
    public int MoneyPerTick => GetMoneyPerTick();
    public float DegradationRate => GetDegradationRate();

    [SerializeField] private GameObject MoneyGainUI;

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        UpdateMaterial();
    }

    void Update()
    {
        
    }

    public void SetTileType(TileType newType)
    {
        tileType = newType;
        
        UpdateMaterial();
    }

    // Money earned per tick for each tile type
    private int GetMoneyPerTick()
    {
        int amount = 0;
        switch (tileType)
        {
            case TileType.Mine:
                amount= MONEY_MINE; // Highest income
                break;
            case TileType.CowField:
                amount= MONEY_COW_FIELD;
                break;
            case TileType.Farm:
                amount= MONEY_FARM;
                break;
            case TileType.Agroforest:
                amount= MONEY_AGROFOREST; // Lowest income but sustainable
                break;
            case TileType.Grass:
            case TileType.Rainforest:
            case TileType.Barren:
                amount= MONEY_NATURAL; // No income
                break;
            default:
                amount= MONEY_NATURAL;
                break;
        }
        amount = Mathf.FloorToInt(amount * PointSystem.Instance.GetEfficiencyMultiplier(tileType));
        return amount;

    }

    // Degradation rate per turn (how fast the tile becomes barren)
    private float GetDegradationRate()
    {
        switch (tileType)
        {
            case TileType.Mine:
                return DEGRADATION_MINE; // Fastest degradation
            case TileType.CowField:
                return DEGRADATION_COW_FIELD;
            case TileType.Farm: 
                return DEGRADATION_FARM;
            case TileType.Agroforest:
                return DEGRADATION_AGROFOREST; // Slowest degradation (sustainable)
            case TileType.Grass:
            case TileType.Rainforest:
                return DEGRADATION_NATURAL_REGEN; // Natural regeneration
            case TileType.Barren:
                return DEGRADATION_NONE; // Already barren
            default:
                return DEGRADATION_NONE;
        }
    }

    // Call this each turn/cycle to generate money and degrade the tile
    // This method is triggered by the Unity event from another script
    public void OnTick()
    {   
        int moneyEarned = GetMoneyPerTick();
        
        // Apply degradation
        currentDegradation += GetDegradationRate();
        // Check if tile should become barren
        if (currentDegradation >= degradationThreshold && tileType != TileType.Barren)
        {
            ConvertToBarren();
        }
        // Natural regeneration for grass/rainforest
        else if (currentDegradation < 0)
        {
            currentDegradation = 0;
        }
        
        // Update material based on current degradation
        UpdateMaterial();
        
        // Broadcast the money earned to any listeners (like PointSystem)
        if (moneyEarned > 0)
        {
            onMoneyEarned?.Invoke(moneyEarned);
            if (MoneyGainUI!=null)
            {
                var ui=Instantiate(MoneyGainUI,transform);
                ui.GetComponent<TileMoneyGainUI>().SetMoneyGainUI(moneyEarned);
            }
        }
    }

    private void ConvertToBarren()
    {
        Debug.Log($"Tile at {transform.position} has become barren!");
        SetTileType(TileType.Barren);
        
        // Instantiate barren prefab on top of the tile
        if (barrenPrefab != null)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * placementHeight; // Slightly above tile
            Instantiate(barrenPrefab, spawnPosition, Quaternion.identity, transform);
        }
        else
        {
            Debug.LogWarning($"Barren prefab not assigned for tile at {transform.position}");
        }
    }

    // Update the tile's material based on current degradation level
    private void UpdateMaterial()
    {
        if (tileRenderer == null) return;
        
        // If tile is barren, use barren material
        if (tileType == TileType.Barren)
        {
            if (barrenMaterial != null)
            {
                tileRenderer.material = barrenMaterial;
            }
            return;
        }
        
        // Calculate degradation percentage (0 to 1)
        float degradationPercent = currentDegradation / degradationThreshold;
        
        // Select material based on degradation level
        Material targetMaterial = null;
        
        if (degradationPercent >= criticalThreshold)
        {
            targetMaterial = criticalMaterial;
        }
        else if (degradationPercent >= degradedThreshold)
        {
            targetMaterial = degradedMaterial;
        }
        else
        {
            targetMaterial = healthyMaterial;
        }
        
        // Apply the material if it exists
        if (targetMaterial != null)
        {
            tileRenderer.material = targetMaterial;
        }
    }

    // Get info about how many turns until barren
    public int GetTurnsUntilBarren()
    {
        if (tileType == TileType.Barren || GetDegradationRate() <= 0)
        {
            return -1; // Never becomes barren or already barren
        }
        
        float turnsRemaining = (degradationThreshold - currentDegradation) / GetDegradationRate();
        return Mathf.CeilToInt(turnsRemaining);
    }

    // Display tile info
    public string GetTileInfo()
    {
        string info = $"Type: {tileType}\n";
        info += $"Income: ${GetMoneyPerTick()}/tick\n";
        info += $"Degradation: {currentDegradation:F1}/{degradationThreshold}\n";
        
        int turnsLeft = GetTurnsUntilBarren();
        if (turnsLeft > 0)
        {
            info += $"Turns until barren: {turnsLeft}";
        }
        else if (turnsLeft == -1 && tileType != TileType.Barren)
        {
            info += "Sustainable";
        }
        
        return info;
    }
}
