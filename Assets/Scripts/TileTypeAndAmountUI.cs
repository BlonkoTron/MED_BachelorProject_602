using UnityEngine;

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
            GameManager.Instance.onGameTick.AddListener(OnGameTick);
        }
        else
        {
            Debug.LogWarning("GameManager not found - tile money gain won't update with game ticks");
        }

        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);

        foreach (Tile tile in allTiles)
        {
           // tile.onTileTypeChanged.AddListener(OnTileChanged);
        }



        CountTiles();
        CalculateMoneyGain();
    }

    private void OnGameTick()
    {
        CountTiles();
        CalculateMoneyGain();
    }

    public void CountTiles()
    {         // Find all tiles
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);

        if (allTiles.Length == 0) return;

        foreach (Tile tile in allTiles)
        {
            switch (tile.Type)
            {
                case TileType.Mine:
                    mineTiles++;
                    break;
                case TileType.CowField:
                    pastureTiles++;
                    break;
                case TileType.Agroforest:
                    agroforestTiles++;
                    break;
                case TileType.Farm:
                    farmTiles++;
                    break;
                case TileType.Grass:
                    grassTiles++;
                    break;
                case TileType.Rainforest:
                    rainforestTiles++;
                    break;
                case TileType.Barren:
                    barrenTiles++;
                    break;
            }
        }
    }

    public void CalculateMoneyGain()
    {
        agroforestMoneyGain = agroforestTiles * gameSettings.MONEY_AGROFOREST;
        farmMoneyGain = farmTiles * gameSettings.MONEY_FARM;
        pastureMoneyGain = pastureTiles * gameSettings.MONEY_COW_FIELD;
        mineMoneyGain = mineTiles * gameSettings.MONEY_MINE;
    }
}
