using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TileChanger : MonoBehaviour
{
    public GameObject highlightObj;

    private GameObject tileUI;
    [SerializeField] private GameObject blankTileUI;
    [SerializeField] private GameObject buildingUI;

    private EventInstance ClickSFX_Open;
    [SerializeField] private EventReference ClickSFX_OpenUI;
    private EventInstance ClickSFX_Close;
    [SerializeField] private EventReference ClickSFX_CloseUI;

    [HideInInspector] public UnityEvent onTileChanged = new UnityEvent();

    private Tile tile;

    private GameSettings gameSettings;

    public bool stopfirstsound = false;

    [SerializeField] private GameObject mineDisabledImage;
    [SerializeField] private GameObject cowDisabledImage;
    [SerializeField] private GameObject agroDisabledImage;
    [SerializeField] private GameObject farmDisabledImage;

    private void Start()
    {

        gameSettings = GameManager.Instance.gameSettings;
        tile = GetComponent<Tile>();

        UpdateUI();

        if (tileUI != null)
        {
            CloseUI();
        }

    }

    public void OnClick()
    {
        //Debug.Log(gameObject.name + " Says: 'Im Clicked'");
        ClickSFX_Open = Audiomanager.instance.PlaySound(ClickSFX_OpenUI, transform.position);
        if (tile.Type != TileType.Barren) 
        {

            tileUI.SetActive(true);
            stopfirstsound = true;
        }

        UpdateDisabledIcons();
    }

    public void CloseUI()
    {
        if (stopfirstsound == true)
        {
            ClickSFX_Close = Audiomanager.instance.PlaySound(ClickSFX_CloseUI, transform.position);
        }
        
        tileUI.GetComponent<Animator>().SetTrigger("Reset");

        tileUI.SetActive(false);

        //Debug.Log(gameObject.name + " Is me and im closing my UI");

    }
    public bool ChangeTile(TileType type)
    {
        if (!IsTileTypeEnabled(type))
        {
            Debug.Log(type + " is disabled!");
            return false;
        }

        if (CanAffordTileChange(type))
        {
            Debug.Log("Changing tile to " + type);

            // Check if we're changing from a rainforest tile
            if (tile.Type == TileType.Rainforest && type != TileType.Rainforest)
            {
                // Apply happiness penalty for changing rainforest
                if (Happiness.Instance != null)
                {
                    int happinessPenalty = gameSettings.HAPPINESS_PENALTY_RAINFOREST_CHANGE;
                    Happiness.Instance.DecreaseHappiness(happinessPenalty);
                    Debug.Log($"Rainforest changed - Happiness decreased by {happinessPenalty}");
                }
                else
                {
                    Debug.LogWarning("Happiness system not found - skipping happiness penalty");
                }
            }

            tile.SetTileType(type, GetTileCost(type));
            CloseUI();
            UpdateUI();
            PointSystem.Instance.SpendMoney(GetTileCost(type));
            onTileChanged.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Can't afford");
            return false;
        }
    }

        public int GetTileCost(TileType type)
    {
        switch (type)
        {
            case TileType.Mine:
                return gameSettings.COST_MINE;
            case TileType.CowField:
                return gameSettings.COST_COW_FIELD;
            case TileType.Farm:
                return gameSettings.COST_FARM;
            case TileType.Agroforest:
                return gameSettings.COST_AGROFOREST;
            case TileType.Grass:
                return gameSettings.COST_NATURAL;
            case TileType.Rainforest:
                return gameSettings.COST_NATURAL;
        }
        return 0;
    }
    public bool CanAffordTileChange(TileType type)
    {
        if (PointSystem.Instance.CurrentMoney >= GetTileCost(type))
        {
            return true;
        } else
        {
            return false;
        }
    }

    private bool IsTileTypeEnabled(TileType type)
    {
        switch (type)
        {
            case TileType.Mine:
                return !gameSettings.DisableMineTiles;
            case TileType.CowField:
                return !gameSettings.DisableCowTiles;
            case TileType.Agroforest:
                return !gameSettings.DisableAgroTiles;
            case TileType.Farm:
                return !gameSettings.DisableFarmTiles;
            default:
                return true;
        }
    }

    private void UpdateDisabledIcons()
    {
        mineDisabledImage.SetActive(!IsTileTypeEnabled(TileType.Mine));
        cowDisabledImage.SetActive(!IsTileTypeEnabled(TileType.CowField));
        agroDisabledImage.SetActive(!IsTileTypeEnabled(TileType.Agroforest));
        farmDisabledImage.SetActive(!IsTileTypeEnabled(TileType.Farm));
    }

    private void UpdateUI()
    {
        switch (tile.Type)
        {
            case TileType.Mine:
            case TileType.CowField:
            case TileType.Farm:
            case TileType.Agroforest:
                tileUI = buildingUI;

                tileUI.GetComponent<TileInfoUI>().UpdateUI();

                return;
            case TileType.Grass:
            case TileType.Rainforest:
            case TileType.Barren:
                tileUI = blankTileUI;
                return;

        }
        UpdateDisabledIcons();
    }


}

