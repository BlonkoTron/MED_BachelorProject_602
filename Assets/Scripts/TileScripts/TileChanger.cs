using Unity.VisualScripting;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Internal;

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

    private Tile tile;

    private GameSettings gameSettings;

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

        }

    }

    public void CloseUI()
    {
        ClickSFX_Close = Audiomanager.instance.PlaySound(ClickSFX_CloseUI, transform.position);

        tileUI.GetComponent<Animator>().SetTrigger("Reset");

        tileUI.SetActive(false);

        //Debug.Log(gameObject.name + " Is me and im closing my UI");

    }

    public bool ChangeTile(TileType type)
    {
        if (CanAffordTileChange(type))
        {
            Debug.Log("Changing tile to " + type);
            tile.SetTileType(type,GetTileCost(type));
            CloseUI();
            UpdateUI();
            PointSystem.Instance.SpendMoney(GetTileCost(type));
            return true;
        } else
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


}
