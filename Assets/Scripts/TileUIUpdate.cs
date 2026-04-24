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

    public enum TileTypeUIAmountOrGrain
    {
       Amount,
       Gain
    }

    public TileTypeUI selectedTileType;
    public TileTypeUIAmountOrGrain selectedAmountOrGrain;

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
                text.text = "x" + tileTypeUI.grassTiles.ToString();
                break;
            case TileTypeUI.Rainforest:
                text.text = "x" + tileTypeUI.rainforestTiles.ToString();
                break;
            case TileTypeUI.Farm:
                if (selectedAmountOrGrain == TileTypeUIAmountOrGrain.Amount)
                {
                    text.text = "x" + tileTypeUI.farmTiles.ToString();
                }
                else
                {
                    text.text = tileTypeUI.farmMoneyGain.ToString();
                }
                break;
            case TileTypeUI.CowField:
                if (selectedAmountOrGrain == TileTypeUIAmountOrGrain.Amount)
                {
                    text.text = "x" + tileTypeUI.pastureTiles.ToString();
                }
                else
                {
                    text.text = tileTypeUI.pastureMoneyGain.ToString();
                }
                break;
            case TileTypeUI.Agroforest:
                if (selectedAmountOrGrain == TileTypeUIAmountOrGrain.Amount)
                {
                    text.text = "x" + tileTypeUI.agroforestTiles.ToString();
                }
                else
                {
                    text.text = tileTypeUI.agroforestMoneyGain.ToString();
                }
                break;
            case TileTypeUI.Barren:
                text.text = "x" + tileTypeUI.barrenTiles.ToString();
                break;
            case TileTypeUI.Mine:
                if (selectedAmountOrGrain == TileTypeUIAmountOrGrain.Amount)
                {
                    text.text = "x" + tileTypeUI.mineTiles.ToString();
                }
                else
                {
                    text.text = tileTypeUI.mineMoneyGain.ToString();
                }
                break;
        }
    }

}
