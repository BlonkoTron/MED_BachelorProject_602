using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TileTypeAndAmountUI : MonoBehaviour
{
    public static TileTypeAndAmountUI Instance { get; private set; }
    [HideInInspector] public int rainforestTiles = 0;
    [HideInInspector] public int agroforestTiles = 0;
    [HideInInspector] public int farmTiles = 0;
    [HideInInspector] public int pastureTiles = 0;
    [HideInInspector] public int mineTiles = 0;
    [HideInInspector] public int grassTiles = 0;
    [HideInInspector] public int barrenTiles = 0;

    [HideInInspector] public float agroforestMoneyGain;
    [HideInInspector] public float farmMoneyGain;
    [HideInInspector] public float pastureMoneyGain;
    [HideInInspector] public float mineMoneyGain;

    public GameSettings gameSettings;
    public GradientColor gradientColor;
    public TMP_Text percentageText;

    [HideInInspector] public UnityEvent onTileUIUpdate = new UnityEvent();

    public int rainforestScore;
    public float rainforestScorePercentage;

    [SerializeField] private int rainforestTileScore = 3;
    [SerializeField] private int agroforestTileScore = 2;
    [SerializeField] private int grassTileScore = 1;
    [SerializeField] private int farmTileScore = 0;
    [SerializeField] private int pastureTileScore = -1;
    [SerializeField] private int mineTileScore = -2;
    [SerializeField] private int barrenTileScore = -3;

    //---- D here
    [Header("Feedback Controllers")]
    public FeedbackTriangleController rainFeedback;
    public FeedbackTriangleController agroFeedback;
    public FeedbackTriangleController farmFeedback;
    public FeedbackTriangleController pastureFeedback;
    public FeedbackTriangleController mineFeedback;
    public FeedbackTriangleController barrenFeedback;
    public FeedbackTriangleController grassFeedback;


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

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onGameTick.AddListener(Recalculate);
        }
        else
        {
            Debug.LogWarning("GameManager not found - tile money gain won't update with game ticks");
        }

        TileChanger[] allTiles = FindObjectsByType<TileChanger>(FindObjectsSortMode.None);

        foreach (TileChanger tile in allTiles)
        {
           tile.onTileChanged.AddListener(Recalculate);
        }

        Recalculate();
    }

    private void Recalculate()
    {
        //D was here
        int oldRain = rainforestTiles;
        int oldAgro = agroforestTiles;
        int oldFarm = farmTiles;
        int oldPasture = pastureTiles;
        int oldMine = mineTiles;
        int oldBarren = barrenTiles;
        int oldGrass = grassTiles;

        CountTiles();
        CalculateMoneyGain();

        onTileUIUpdate.Invoke();
        // and here
        if (rainforestTiles != oldRain) rainFeedback?.TriggerChange(rainforestTiles - oldRain);
        if (agroforestTiles != oldAgro) agroFeedback?.TriggerChange(agroforestTiles - oldAgro);
        if (farmTiles != oldFarm) farmFeedback?.TriggerChange(farmTiles - oldFarm);
        if (pastureTiles!= oldPasture) pastureFeedback?.TriggerChange(pastureTiles - oldPasture);
        if (mineTiles != oldMine) mineFeedback?.TriggerChange(mineTiles - oldMine);
        if (barrenTiles != oldBarren) barrenFeedback?.TriggerChange(barrenTiles - oldBarren);
        if (grassTiles != oldGrass) grassFeedback?.TriggerChange(grassTiles - oldGrass);

    }

    public void CountTiles()
    {   
        // Find all tiles
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);

        if (allTiles.Length == 0) return;

        mineTiles = 0;
        pastureTiles = 0;
        agroforestTiles = 0;
        farmTiles = 0;
        grassTiles = 0;
        rainforestTiles = 0;
        barrenTiles = 0;
        rainforestScore = 0;

        foreach (Tile tile in allTiles)
        {
            switch (tile.Type)
            {
                case TileType.Mine:
                    mineTiles++;
                    rainforestScore += mineTileScore;
                    break;
                case TileType.CowField:
                    pastureTiles++;
                    rainforestScore += pastureTileScore;
                    break;
                case TileType.Agroforest:
                    agroforestTiles++;
                    rainforestScore += agroforestTileScore;
                    break;
                case TileType.Farm:
                    farmTiles++;
                    rainforestScore += farmTileScore;
                    break;
                case TileType.Grass:
                    grassTiles++;
                    rainforestScore += grassTileScore;
                    break;
                case TileType.Rainforest:
                    rainforestTiles++;
                    rainforestScore += rainforestTileScore;
                    break;
                case TileType.Barren:
                    barrenTiles++;
                    rainforestScore += barrenTileScore;
                    break;
            }
        }

        int totalTiles = allTiles.Length;

        int maxRainforestScore = totalTiles * rainforestTileScore;
        int minRainforestScore = totalTiles * barrenTileScore;

        rainforestScorePercentage = (float)(rainforestScore - minRainforestScore) / (maxRainforestScore - minRainforestScore) * 100;
        gradientColor.SetValue(rainforestScorePercentage);
        percentageText.text = rainforestScorePercentage.ToString("F1") + "%";

        // Debug.Log(rainforestScorePercentage);
    }   

    public void CalculateMoneyGain()
    {
        agroforestMoneyGain = agroforestTiles * gameSettings.MONEY_AGROFOREST*PointSystem.Instance.agroforestEfficiencyMultiplier;
        farmMoneyGain = farmTiles * gameSettings.MONEY_FARM*PointSystem.Instance.farmEfficiencyMultiplier;
        pastureMoneyGain = pastureTiles * gameSettings.MONEY_COW_FIELD * PointSystem.Instance.cowfieldEfficiencyMultiplier;
        mineMoneyGain = mineTiles * gameSettings.MONEY_MINE * PointSystem.Instance.mineEfficiencyMultiplier;
    }
    public float GetTotalMoneyGain()
    {        
        return agroforestMoneyGain + farmMoneyGain + pastureMoneyGain + mineMoneyGain;
    }
}
