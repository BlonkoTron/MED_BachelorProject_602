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
    [SerializeField] private Animator tileAnimator;

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
        InteractionManager.Instance.closeUI.AddListener(CloseUI);

        UpdateUI();

        if (tileUI != null)
        {
            CloseUI();
        }

    }

    public void OnClick()
    {
        InteractionManager.Instance.closeUI.Invoke();
        //Debug.Log(gameObject.name + " Says: 'Im Clicked'");
        ClickSFX_Open = Audiomanager.instance.PlaySound(ClickSFX_OpenUI, transform.position);
        if (tile.Type != TileType.Barren) 
        {

            tileAnimator.SetBool("Highlighted", true);

            tileUI.SetActive(true);
            stopfirstsound = true;
        }

        UpdateDisabledIcons();
    }

    public void CloseUI()
    {
        if (tileUI.activeSelf) 
        {
            if (stopfirstsound == true)
            {
                ClickSFX_Close = Audiomanager.instance.PlaySound(ClickSFX_CloseUI, transform.position);
            }

            tileAnimator.SetBool("Highlighted", false);

            tileUI.GetComponent<Animator>().SetTrigger("Reset");

            tileUI.SetActive(false);

            //Debug.Log(gameObject.name + " Is me and im closing my UI");
        }


    }
    public bool ChangeTile(TileType type)
    {
        // 🚫 Block if tile type is disabled
        if (!IsTileTypeEnabled(type))
        {
            Debug.Log(type + " is currently disabled!");
            return false;
        }

        if (CanAffordTileChange(type))
        {
            Debug.Log("Changing tile to " + type);

            if (tile.Type == TileType.Rainforest && type != TileType.Rainforest)
            {
                if (Happiness.Instance != null)
                {
                    int happinessPenalty = gameSettings.HAPPINESS_PENALTY_RAINFOREST_CHANGE;
                    Happiness.Instance.DecreaseHappiness(happinessPenalty);
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
                return !gameSettings.DisableMineTilesGamesetting;
            case TileType.CowField:
                return !gameSettings.DisableCowTilesGamesetting;
            case TileType.Agroforest:
                return !gameSettings.DisableAgroTilesGamesetting;
            case TileType.Farm:
                return !gameSettings.DisableFarmTilesGamesetting;
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


    private void OnDestroy()
    {
        InteractionManager.Instance.closeUI.RemoveListener(CloseUI);
    }

}

