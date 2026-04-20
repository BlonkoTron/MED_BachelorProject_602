using UnityEngine;
using UnityEngine.UI;

public class TileButtons : MonoBehaviour
{
    [SerializeField] private TileChanger ParentTile;

    [SerializeField] private TileType tileType;

    private Button button;

    private PointSystem pointSystem;

    private void Start()
    {
        button = GetComponent<Button>();
        pointSystem = PointSystem.Instance;
        pointSystem.onMoneyEarned.AddListener(UpdateButtonInteractability);
        pointSystem.onMoneyLost.AddListener(UpdateButtonInteractability);
        pointSystem.onMoneySpent.AddListener(UpdateButtonInteractability);

    }

    public void OnClick()
    {
        ParentTile.ChangeTile(tileType);
    }

    private void UpdateButtonInteractability(int money)
    {

    }
    private void OnDestroy()
    {
        pointSystem.onMoneyEarned.RemoveListener(UpdateButtonInteractability);
        pointSystem.onMoneyLost.RemoveListener(UpdateButtonInteractability);
        pointSystem.onMoneySpent.RemoveListener(UpdateButtonInteractability);
    }


}
