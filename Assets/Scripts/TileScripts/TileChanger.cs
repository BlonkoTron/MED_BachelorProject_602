using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Internal;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Events;

public class TileChanger : MonoBehaviour
{
    public GameObject highlightObj;

    private GameObject tileUI;
    [SerializeField] private GameObject blankTileUI;
    [SerializeField] private GameObject buildingUI;
    [SerializeField] private GameObject dustPrefab;

    private Animator topAnimator;

    private EventInstance ClickSFX_Open;
    [SerializeField] private EventReference ClickSFX_OpenUI;
    private EventInstance ClickSFX_Close;
    [SerializeField] private EventReference ClickSFX_CloseUI;

    [HideInInspector] public UnityEvent onTileChanged = new UnityEvent();

    private Tile tile;

    private GameSettings gameSettings;

    private Collider tileCollider;

    public bool stopfirstsound = false;
    private bool hasWaited;


    [SerializeField] private GameObject mineDisabledImage;
    [SerializeField] private GameObject cowDisabledImage;
    [SerializeField] private GameObject agroDisabledImage;
    [SerializeField] private GameObject farmDisabledImage;

    private TileType tileType_toChange;

    private void Start()
    {
        gameSettings = GameManager.Instance.gameSettings;
        tile = GetComponent<Tile>();
        InteractionManager.Instance.closeUI.AddListener(CloseUI);

        UpdateUI();

        tileCollider = GetComponent<Collider>();

        topAnimator = GetComponent<Animator>();

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

            if (TutorialManager.instance != null)
            {
                if (!TutorialManager.instance.hasFirstClicked)
                {
                    TutorialManager.instance.hasFirstClicked = true;
                    TutorialManager.instance.UpdateTutorial();
                }
            }


            topAnimator.SetBool("Highlighted", true);

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

            topAnimator.SetBool("Highlighted", false);

            tileUI.GetComponent<Animator>().SetTrigger("Reset");

            tileUI.SetActive(false);

            //Debug.Log(gameObject.name + " Is me and im closing my UI");
        }


    }
    public bool ChangeTile(TileType type)
    {
        // 🚫 Block if tile type is disabled

        tileCollider.enabled = false;

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
            
            CloseUI();

            tileType_toChange = type;

            topAnimator.SetTrigger("Swap");

            // Calculate the cost difference
            int targetCost = GetTileCost(type);
            int currentCost = GetTileCost(tile.Type);
            int costDifference = targetCost - currentCost;
            
            if (costDifference > 0)
            {
                // Upgrading: spend the difference
                PointSystem.Instance.SpendMoney(costDifference);
            }
            else if (costDifference < 0)
            {
                // Downgrading: get money back
                PointSystem.Instance.AddMoney(-costDifference);
            }
            // If costDifference == 0, no money exchange needed
            
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
        
        int targetCost = GetTileCost(type);
        int currentCost = GetTileCost(tile.Type);
        
        // If downgrading (target is cheaper than current), always allow it
        if (targetCost < currentCost)
        {
            return true;
        }
        
        // If upgrading or lateral move, check if player can afford it
        if (PointSystem.Instance.CurrentMoney >= targetCost)
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
                return !GameManager.Instance.DisableMineTilesGamesetting;
            case TileType.CowField:
                return !GameManager.Instance.DisableCowTilesGamesetting;
            case TileType.Agroforest:
                return !GameManager.Instance.DisableAgroTilesGamesetting;
            case TileType.Farm:
                return !GameManager.Instance.DisableFarmTilesGamesetting;
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

    public void SwapMethod()
    {

        tile.SetTileType(tileType_toChange, GetTileCost(tileType_toChange));
        UpdateUI();
        tileCollider.enabled = true;

        if (TutorialManager.instance != null) 
        { 
            if (!TutorialManager.instance.hasFirstBuilded)
            {
                TutorialManager.instance.hasFirstBuilded = true;
                TutorialManager.instance.UpdateTutorial();
            }
        }

    }

    public void CreateDust()
    {
        Debug.Log("BABOOM");
        Instantiate(dustPrefab, transform);
    }

    private void OnDestroy()
    {
        InteractionManager.Instance.closeUI.RemoveListener(CloseUI);
    }

}

