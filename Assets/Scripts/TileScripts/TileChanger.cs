using Unity.VisualScripting;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using NUnit.Framework.Internal;

public class TileChanger : MonoBehaviour
{
    public GameObject highlightObj;

    [SerializeField] private GameObject tileUI;
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
            tile.SetTileType(type);
            CloseUI();
            UpdateUI();
            return true;
        } else
        {
            Debug.Log("Can't afford");
            return false;
        }

    }
    public bool CanAffordTileChange(TileType type)
    {
        var money = PointSystem.Instance.CurrentMoney;
        switch (type)
        {
            case TileType.Mine:
                if (money >= gameSettings.COST_MINE) { return true; };
                break;
            case TileType.CowField:
                if (money >= gameSettings.COST_COW_FIELD) { return true; };
                break;
            case TileType.Farm:
                if (money >= gameSettings.COST_FARM) { return true; };
                break;
            case TileType.Agroforest:
                if (money >= gameSettings.COST_AGROFOREST) { return true; };
                break;
            case TileType.Grass:
                if (money >= gameSettings.COST_NATURAL) { return true; };
                break;
            case TileType.Rainforest:
                if (money >= gameSettings.COST_NATURAL) { return true; };
                break;
            case TileType.Barren:
                return true;
        }
        return false;
        
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
