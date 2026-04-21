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

    public bool DisableMineTiles;
    public bool DisableCowTiles;
    public bool DisableAgroTiles;
    public bool DisableFarmTiles;

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
    }


    private void OnDestroy()
    {
        InteractionManager.Instance.closeUI.RemoveListener(CloseUI);
    }

}

