using Unity.VisualScripting;
using UnityEngine;

public class TileChanger : MonoBehaviour
{
    public GameObject highlightObj;

    [SerializeField] private GameObject tileUI;
    [SerializeField] private GameObject blankTileUI;
    [SerializeField] private GameObject buildingUI;


    private Tile tile;

    private void Start()
    {
   

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

        if (tile.Type != TileType.Barren) 
        {

            tileUI.SetActive(true);

        }

    }

    public void CloseUI()
    {

        tileUI.GetComponent<Animator>().SetTrigger("Reset");

        tileUI.SetActive(false);

        //Debug.Log(gameObject.name + " Is me and im closing my UI");

    }

    public void ChangeTile(TileType type)
    {
        Debug.Log("Changing tile to " + type);
        tile.SetTileType(type);
        CloseUI();
        UpdateUI();

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
