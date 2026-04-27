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


    //Audio
    //Click open UI
    private EventInstance ClickSFX_Open;
    [SerializeField] private EventReference ClickSFX_Open_REF;
    //Click close UI
    private EventInstance ClickSFX_Close;
    [SerializeField] private EventReference ClickSFX_Close_REF;
    //Flip tile
    private EventInstance TileFlip_SFX;
    [SerializeField] private EventReference TileFlip_SFX_REF;



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
    private int actualCostForUI; // Tracks actual money change for visual display

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
        ClickSFX_Open = Audiomanager.instance.PlaySound(ClickSFX_Open_REF, transform.position);
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
                ClickSFX_Close = Audiomanager.instance.PlaySound(ClickSFX_Close_REF, transform.position);
            }

            topAnimator.SetBool("Highlighted", false);

            tileUI.GetComponent<Animator>().SetTrigger("Reset");

            tileUI.SetActive(false);

            //Debug.Log(gameObject.name + " Is me and im closing my UI");
        }


    }
    public bool ChangeTile(TileType type)
    {
        CloseUI();
        //tileCollider.enabled = false;

        if (!IsTileTypeEnabled(type))
        {
            Debug.Log(type + " is currently disabled!");
            return false;
        }

        if (CanAffordTileChange(type))
        {
            Debug.Log("Changing tile to " + type);
            //Play sfx
            TileFlip_SFX = Audiomanager.instance.PlaySound(TileFlip_SFX_REF, transform.position);

           

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

            // Special handling when converting TO natural tiles
            if (type == TileType.Grass || type == TileType.Rainforest)
            {
                // Only give the flat demolition bonus, not the tile's full value
                int demolitionBonus = Mathf.Abs(gameSettings.COST_NATURAL); // 10 gold
                PointSystem.Instance.AddMoney(demolitionBonus);
                actualCostForUI = -demolitionBonus; // Negative for visual display (shows as gain)
            }
            else
            {
                // Normal cost calculation for non-natural tiles
                int targetCost = GetTileCost(type);
                int currentCost = GetTileCost(tile.Type);
                int costDifference = targetCost - currentCost;
                
                if (costDifference > 0)
                {
                    // Upgrading: spend the difference
                    if (!PointSystem.Instance.SpendMoney(costDifference))
                    {
                        Debug.LogError("Failed to spend money despite CanAffordTileChange check!");
                        //tileCollider.enabled = true;
                        return false;
                    }
                    actualCostForUI = costDifference; // Positive (cost)
                }
                else if (costDifference < 0)
                {
                    // Downgrading: get money back (difference between tiles)
                    PointSystem.Instance.AddMoney(-costDifference);
                    actualCostForUI = costDifference; // Negative (gain)
                }
                else
                {
                    actualCostForUI = 0; // No change
                }
            }
            
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
            case TileType.Rainforest:
                // Natural tiles have 0 cost for purchasing calculations
                // The demolition refund is handled separately in ChangeTile()
                return 0;
        }
        return 0;
    }
    public bool CanAffordTileChange(TileType type)
    {
        
        int targetCost = GetTileCost(type);
        int currentCost = GetTileCost(tile.Type);
        int costDifference = targetCost - currentCost;
        
        // If downgrading (target is cheaper than current), always allow it
        if (costDifference <= 0)
        {
            return true;
        }
        
        // If upgrading, check if player can afford the difference
        if (PointSystem.Instance.CurrentMoney >= costDifference)
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

        tile.SetTileType(tileType_toChange, actualCostForUI);
        UpdateUI();
        //tileCollider.enabled = true;

        CloseUI();

        if (TutorialManager.instance != null) 
        { 
            if (!TutorialManager.instance.hasFirstBuilded)
            {
                TutorialManager.instance.hasFirstBuilded = true;
                TutorialManager.instance.UpdateTutorial();
            }
        }
        onTileChanged.Invoke();
    }

    public void CreateDust()
    {
        Debug.Log("BABOOM");
        CloseUI();
        Instantiate(dustPrefab, transform);
    }

    private void OnDestroy()
    {
        InteractionManager.Instance.closeUI.RemoveListener(CloseUI);
    }

}

