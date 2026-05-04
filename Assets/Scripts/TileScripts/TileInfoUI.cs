using TMPro;
using UnityEngine;

public class TileInfoUI : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] TMP_Text info_text;
    [SerializeField] TMP_Text sellIncomeText;

    private void Awake()
    {
        GameManager.Instance.onGameTick.AddListener(UpdateUI);
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        info_text.text = tile.GetTileInfo();
        sellIncomeText.text ="+"+Mathf.Abs(GameManager.Instance.gameSettings.COST_NATURAL).ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameTick.RemoveListener(UpdateUI);
    }

}
