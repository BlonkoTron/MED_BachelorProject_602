using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileButtons : MonoBehaviour
{
    [SerializeField] private TileChanger ParentTile;

    [SerializeField] private TileType tileType;

    private Button button;

    [SerializeField] private TMP_Text costText;

    private PointSystem pointSystem;

    [SerializeField] private Color32 unAvailableColor;

    [SerializeField] private Color32 startColor;

    private void Start()
    {
        button = GetComponent<Button>();
        pointSystem = PointSystem.Instance;
        pointSystem.onMoneyEarned.AddListener(UpdateButtonInteractability);
        pointSystem.onMoneyLost.AddListener(UpdateButtonInteractability);
        pointSystem.onMoneySpent.AddListener(UpdateButtonInteractability);
        ParentTile.onTileChanged.AddListener(UpdateButtonInteractability);
        UpdateButtonInteractability(pointSystem.CurrentMoney);
        if (costText!=null)
        {
            costText.text = ParentTile.GetTileCost(tileType).ToString();
        }
    }

    public void OnClick()
    {
        ParentTile.ChangeTile(tileType);
    }
    private void UpdateButtonInteractability()
    {
        int targetCost = ParentTile.GetTileCost(tileType);
        int currentCost = ParentTile.GetTileCost(ParentTile.GetComponent<Tile>().Type);
        UpdateTextColor();
        // If downgrading (target is cheaper), always allow it
        if (targetCost < currentCost)
        {
            button.interactable = true;
        }
        else
        {
            // If upgrading, check if player can afford it
            button.interactable = ParentTile.CanAffordTileChange(tileType);
        }

        if (tileType == TileType.Barren)
        {
            button.interactable = false;
            return;
        }
    }

    private void UpdateButtonInteractability(int money)
   {
    int targetCost = ParentTile.GetTileCost(tileType);
    int currentCost = ParentTile.GetTileCost(ParentTile.GetComponent<Tile>().Type);
    UpdateTextColor();
     // If downgrading (target is cheaper), always allow it
        if (targetCost < currentCost)
        {
            button.interactable = true;
        }
        else
        {
            // If upgrading, check if player can afford it
            button.interactable = ParentTile.CanAffordTileChange(tileType);
        }

        if (tileType == TileType.Barren)
        {
            button.interactable = false;
            return;
        }
    }
    private void UpdateTextColor()
    {
        if (ParentTile.CanAffordTileChange(tileType) && costText != null)
        {
            costText.color = startColor;
        }
        else if (costText!=null)
        {
            costText.color = unAvailableColor;

        }

    }
    private void OnDestroy()
    {
        pointSystem.onMoneyEarned.RemoveListener(UpdateButtonInteractability);
        pointSystem.onMoneyLost.RemoveListener(UpdateButtonInteractability);
        pointSystem.onMoneySpent.RemoveListener(UpdateButtonInteractability);
        ParentTile.onTileChanged.RemoveListener(UpdateButtonInteractability);
    }


}
