using TMPro;
using UnityEngine;

public class TileInfoUI : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] TMP_Text info_text;

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
    }

    private void OnDestroy()
    {
        GameManager.Instance.onGameTick.RemoveListener(UpdateUI);
    }

}
