using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TileTypeAndAmountUI : MonoBehaviour
{
    public static TileTypeAndAmountUI Instance { get; private set; }
    public int rainforestTiles = 0;
    public int agroforestTiles = 0;
    public int farmTiles = 0;
    public int pastureTiles = 0;
    public int mineTiles = 0;
    public int grassTiles = 0;
    public int barrenTiles = 0;

    public float agroforestMoneyGain;
    public float farmMoneyGain;
    public float pastureMoneyGain;
    public float mineMoneyGain;

    public GameSettings gameSettings;
    public GradientColor gradientColor;
    public TMP_Text percentageText;

    [HideInInspector] public UnityEvent onTileUIUpdate = new UnityEvent();

    public int rainforestScore;
    public float rainforestScorePercent;


    private int rainforestTileScore = 3;
    private int agroforestTileScore = 2;
    private int farmTileScore = 0;
    private int pastureTileScore = -1;
    private int grassTileScore = 1;
    private int barrenTileScore = -3;
    private int mineTileScore = -2;

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
        CountTiles();
        CalculateMoneyGain();

        onTileUIUpdate.Invoke();
    }

    public void CountTiles()
    {         // Find all tiles
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

        rainforestScorePercent = (float)(rainforestScore - minRainforestScore) / (maxRainforestScore - minRainforestScore)*100;
        gradientColor.SetValue(rainforestScorePercent);
        percentageText.text = rainforestScorePercent.ToString("F1") + "%";
        Debug.Log(rainforestScorePercent);
    }

    public void CalculateMoneyGain()
    {
        agroforestMoneyGain = agroforestTiles * gameSettings.MONEY_AGROFOREST;
        farmMoneyGain = farmTiles * gameSettings.MONEY_FARM;
        pastureMoneyGain = pastureTiles * gameSettings.MONEY_COW_FIELD;
        mineMoneyGain = mineTiles * gameSettings.MONEY_MINE;
    }



}
