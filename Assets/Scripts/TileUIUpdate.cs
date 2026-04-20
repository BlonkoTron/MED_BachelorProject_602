using TMPro;
using UnityEngine;

public class TileUIUpdate : MonoBehaviour
{
    private TMP_Text text;

    public TileTypeAndAmountUI tileTypeUI;

    public enum TileTypeUI
    {
        Grass,
        Rainforest,
        Farm,
        CowField,
        Agroforest,
        Barren,
        Mine
    }

    public TileTypeUI selectedTileType;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        UpdateText();
        tileTypeUI.onTileUIUpdate.AddListener(UpdateText);

       
    }

    private void UpdateText()
    {
        switch (selectedTileType)
        {
            case TileTypeUI.Grass:
                text.text = tileTypeUI.grassTiles.ToString() + "x";
                break;
            case TileTypeUI.Rainforest:
                text.text = tileTypeUI.rainforestTiles.ToString() + "x";
                break;
            case TileTypeUI.Farm:
                text.text = tileTypeUI.farmTiles.ToString() + "x = " + tileTypeUI.farmMoneyGain.ToString();
                break;
            case TileTypeUI.CowField:
                text.text = tileTypeUI.pastureTiles.ToString() + "x = " + tileTypeUI.pastureMoneyGain.ToString();
                break;
            case TileTypeUI.Agroforest:
                text.text = tileTypeUI.agroforestTiles.ToString() + "x = " + tileTypeUI.agroforestMoneyGain.ToString();
                break;
            case TileTypeUI.Barren:
                text.text = tileTypeUI.barrenTiles.ToString() + "x";
                break;
            case TileTypeUI.Mine:
                text.text = tileTypeUI.mineTiles.ToString() + "x = " + tileTypeUI.mineMoneyGain.ToString();
                break;
        }
    }

}
