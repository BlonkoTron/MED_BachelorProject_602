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
    
    // Event that broadcasts when money is earned
    [HideInInspector] public UnityEvent<int> onMoneyEarned = new UnityEvent<int>();
    
    private Renderer tileRenderer;

    // Tile Properties
    public TileType Type => tileType;
    public float CurrentDegradation => currentDegradation;
    public bool IsBarren => tileType == TileType.Barren;
    public int MoneyPerTick => GetMoneyPerTick();
    public float DegradationRate => GetDegradationRate();

    void Start()
    {
        tileRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        
    }

    public void SetTileType(TileType newType)
    {
        tileType = newType;
        currentDegradation = 0f;
    }

    // Money earned per tick for each tile type
    private int GetMoneyPerTick()
    {
        switch (tileType)
        {
            case TileType.Mine:
                return 100; // Highest income
            case TileType.CowField:
                return 60;
            case TileType.Farm:
                return 50;
            case TileType.Agroforest:
                return 30; // Lowest income but sustainable
            case TileType.Grass:
            case TileType.Rainforest:
            case TileType.Barren:
                return 0; // No income
            default:
                return 0;
        }
    }

    // Degradation rate per turn (how fast the tile becomes barren)
    private float GetDegradationRate()
    {
        switch (tileType)
        {
            case TileType.Mine:
                return 50f; // Fastest degradation
            case TileType.CowField:
                return 34f;
            case TileType.Farm: 
                return 25f;
            case TileType.Agroforest:
                return 10f; // Slowest degradation (sustainable)
            case TileType.Grass:
            case TileType.Rainforest:
                return -10f; // Natural regeneration
            case TileType.Barren:
                return 0f; // Already barren
            default:
                return 0f;
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
        
        // Broadcast the money earned to any listeners (like PointSystem)
        if (moneyEarned > 0)
        {
            onMoneyEarned?.Invoke(moneyEarned);
        }
    }

    private void ConvertToBarren()
    {
        Debug.Log($"Tile at {transform.position} has become barren!");
        SetTileType(TileType.Barren);
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
