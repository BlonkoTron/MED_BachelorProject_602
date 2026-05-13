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
    private GameSettings gameSettings;
    [Header("Tile Configuration")]
    [SerializeField] public TileType tileType = TileType.Grass;
    [SerializeField] private GameObject addonAttachPoint;
    [SerializeField] private GameObject detailTop;

    public GameObject currentAddOn;

    [Header("Tile Types")]
    [SerializeField] private GameObject barrenAddOnPrefab;
    [SerializeField] private GameObject farmAddOnPrefab;
    [SerializeField] private GameObject mineAddOnPrefab;
    [SerializeField] private GameObject cowfieldAddOnPrefab;
    [SerializeField] private GameObject agroforestAddOnPrefab;
    [SerializeField] private GameObject rainforestAddOnPrefab;
    [SerializeField] private GameObject grasslandsAddOnPrefab;
    [SerializeField] private Material barrenDetailMat;
    [SerializeField] private Material farmDetailMat;
    [SerializeField] private Material mineDetailMat;
    [SerializeField] private Material cowfieldDetailMat;
    [SerializeField] private Material agroforestDetailMat;
    [SerializeField] private Material rainforestDetailmat;
    [SerializeField] private Material grasslandsDetailMat;

    private Material currentDetailMat;

    [Header("Degradation")]
    [SerializeField] public float currentDegradation = 0f;
    [SerializeField] private float degradationThreshold = 100f;
    [SerializeField] private GameObject oneTurnWarningPrefab; // Object to spawn when 1 turn til barren
    private bool hasSpawnedWarning = false; // Track if warning was already spawned

    [Header("Materials")]
    [SerializeField] private Material healthyMaterial;
    [SerializeField] private Material degradedMaterial;
    [SerializeField] private Material criticalMaterial;
    [SerializeField] private Material barrenMaterial;

    [Header("Material Thresholds (% of max degradation)")]
    [Range(0f, 1f)]
    [SerializeField] private float degradedThreshold = 0.33f; // 33% degraded
    [Range(0f, 1f)]
    [SerializeField] private float criticalThreshold = 0.66f; // 66% degraded
    
    // Event that broadcasts when money is earned
    [HideInInspector] public UnityEvent<int> onMoneyEarned = new UnityEvent<int>();
    [HideInInspector] public UnityEvent<TileType> onTileChanged;

    [SerializeField] private float warningHeight = 2f; // Height of the warning animation
    private Renderer tileRenderer;

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
        SetTileType(tileType);
        currentDetailMat = detailTop.GetComponent<Renderer>().material;
        gameSettings = GameManager.Instance.gameSettings;
    }

    public void SetTileType(TileType newType)
    {
        if (newType == tileType) return;
        tileType = newType;
        
        // Reset warning flag when tile type changes
        hasSpawnedWarning = false;
        
        UpdateMaterial();
        UpdateAddonPrefab(newType);
        UpdateDetailMaterial(newType);

        onTileChanged.Invoke(tileType);
    }
    public void SetTileType(TileType newType, int cost)
    {
        if (newType == tileType) return;
        tileType = newType;

        // Reset warning flag when tile type changes
        hasSpawnedWarning = false;

        UpdateMaterial();
        UpdateAddonPrefab(newType);
        UpdateDetailMaterial(newType);
        if (MoneyGainUI != null)
        {
            var ui = Instantiate(MoneyGainUI, transform);
            ui.GetComponent<TileMoneyGainUI>().SetMoneyGainUI(cost*-1);
        }
        onTileChanged.Invoke(tileType);
    }

    private void UpdateAddonPrefab(TileType type)
    {
        // clear current add-on
        Destroy(currentAddOn);
        // spawn new
        GameObject newAddOnType;
        switch (type)
        {
            case TileType.Mine:
                newAddOnType = mineAddOnPrefab;
                break;
            case TileType.CowField:
                newAddOnType = cowfieldAddOnPrefab;
                break;
            case TileType.Farm:
                newAddOnType = farmAddOnPrefab;
                break;
            case TileType.Agroforest:
                newAddOnType = agroforestAddOnPrefab;
                break;
            case TileType.Grass:
                newAddOnType = grasslandsAddOnPrefab;
                break;
            case TileType.Rainforest:
                newAddOnType = rainforestAddOnPrefab;
                break;
            case TileType.Barren:
                newAddOnType = barrenAddOnPrefab;
                break;
            default:
                newAddOnType = grasslandsAddOnPrefab;
                break;
        }
        if (newAddOnType == null) return;
        currentAddOn= Instantiate(newAddOnType, addonAttachPoint.transform);
        
    }
    private void UpdateDetailMaterial(TileType type)
    {
        Material newDetailMat;
        switch (type)
        {
            case TileType.Mine:
                newDetailMat = mineDetailMat;
                break;
            case TileType.CowField:
                newDetailMat = cowfieldDetailMat;
                break;
            case TileType.Farm:
                newDetailMat = farmDetailMat;
                break;
            case TileType.Agroforest:
                newDetailMat = agroforestDetailMat;
                break;
            case TileType.Grass:
                newDetailMat = grasslandsDetailMat;
                break;
            case TileType.Rainforest:
                newDetailMat = rainforestDetailmat;
                break;
            case TileType.Barren:
                newDetailMat = barrenDetailMat;
                break;
            default:
                newDetailMat = grasslandsDetailMat; ;
                break;
        }
        currentDetailMat=detailTop.GetComponent<Renderer>().material = newDetailMat;
    }

    // Money earned per tick for each tile type
    private int GetMoneyPerTick()
    {
        int amount = 0;
        switch (tileType)
        {
            case TileType.Mine:
                amount=gameSettings.MONEY_MINE; // Highest income
                break;
            case TileType.CowField:
                amount=gameSettings.MONEY_COW_FIELD;
                break;
            case TileType.Farm:
                amount=gameSettings.MONEY_FARM;
                break;
            case TileType.Agroforest:
                amount=gameSettings.MONEY_AGROFOREST; // Lowest income but sustainable
                break;
            case TileType.Grass:
            case TileType.Rainforest:
            case TileType.Barren:
                amount= gameSettings.MONEY_NATURAL; // No income
                break;
            default:
                amount= gameSettings.MONEY_NATURAL;
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
                return gameSettings.DEGRADATION_MINE * Eventmanager_NEWSETUP.instance.MineDEGRADATION_Mult; // Fastest degradation
            case TileType.CowField:
                return gameSettings.DEGRADATION_COW_FIELD * Eventmanager_NEWSETUP.instance.CowDEGRADATION_Mult;
            case TileType.Farm: 
                return gameSettings.DEGRADATION_FARM * Eventmanager_NEWSETUP.instance.FarmDEGRADATION_Mult;
            case TileType.Agroforest:
                return gameSettings.DEGRADATION_AGROFOREST * Eventmanager_NEWSETUP.instance.AgroDEGRADATION_Mult; // Slowest degradation (sustainable)
            case TileType.Grass:
            case TileType.Rainforest:
                return gameSettings.DEGRADATION_NATURAL_REGEN; // Natural regeneration
            case TileType.Barren:
                
                return gameSettings.DEGRADATION_NONE; // Already barren
            default:
                return gameSettings.DEGRADATION_NONE;
        }
    }

    // Call this each turn/cycle to generate money and degrade the tile
    // This method is triggered by the Unity event from GameManager script
    public void OnTick()
    {
        // Apply degradation
        currentDegradation += GetDegradationRate();
        // Check if tile should become barren
        if (currentDegradation >= degradationThreshold && tileType != TileType.Barren)
        {
            SetTileType(TileType.Barren);
            return;
        }
        // Natural regeneration for grass/rainforest
        else if (currentDegradation <= 0)
        {
            currentDegradation = 0;
            // Grassland converts back to rainforest when fully healed
            if (tileType == TileType.Grass)
            {
                SetTileType(TileType.Rainforest);
                return;
            }
        }
        // Update material based on current degradation
        UpdateMaterial();

        // Check if next turn will make it barren 
        if (!hasSpawnedWarning && oneTurnWarningPrefab != null && tileType != TileType.Barren && tileType != TileType.Grass)
        {
            if (GetTurnsUntilBarren()<=1 && GetDegradationRate() > 0)
            {
                SpawnWarningSign();
            }
        }
        // Broadcast the money earned to any listeners (like PointSystem)
        int moneyEarned = GetMoneyPerTick();
        if (moneyEarned > 0)
        {
            onMoneyEarned?.Invoke(moneyEarned);
            SpawnMoneyGainUI(moneyEarned);
        }
    }
    private void SpawnMoneyGainUI(int moneyGain)
    {
        if (MoneyGainUI != null)
        {
            var ui = Instantiate(MoneyGainUI, transform);
            ui.GetComponent<TileMoneyGainUI>().SetMoneyGainUI(moneyGain);
        }
    }
    private void SpawnWarningSign() 
    {
        // Instantiate warning object
        var warning = Instantiate(oneTurnWarningPrefab, transform);
        warning.transform.localPosition = Vector3.up * warningHeight;
        warning.transform.localRotation = Quaternion.Euler(0, -90, 0);
        warning.transform.localScale = Vector3.one;
        hasSpawnedWarning = true;
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
                GameManager.Instance.FirstBarren();
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
        string info = $"Type: {GetTileName(tileType)}\n";
        info += $"Indtægt: ${GetMoneyPerTick()}/dag\n";
        info += $"Nedbrydning: {currentDegradation:F1}/{degradationThreshold}\n";
        
        int turnsLeft = GetTurnsUntilBarren();
        if (turnsLeft > 0)
        {
            info += $"Øde om {turnsLeft} dage";
        }
        else if (turnsLeft == -1 && tileType != TileType.Barren)
        {
            info += "Bæredygtig";
        }
        
        return info;
    }

    // Public method to reset warning flag (called by TileWarning when destroyed)
    public void ResetWarningFlag()
    {
        hasSpawnedWarning = false;
    }
    private string GetTileName(TileType type)
    {
        switch (tileType)
        {
            case TileType.Mine:
                return "Mine";
            case TileType.CowField:
                return "Kvægfarm";
            case TileType.Farm:
                return "Landbrug";
            case TileType.Agroforest:
                return "Skovlandbrug"; // Slowest degradation (sustainable)
            case TileType.Grass:
            case TileType.Rainforest:
                return "Regnskov"; // Natural regeneration
            case TileType.Barren:
                return "øde"; // Already barren
            default:
                return "";
        }
    }
}