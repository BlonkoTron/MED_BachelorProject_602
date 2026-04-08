using TMPro;
using UnityEngine;

public class TileInfoUI : MonoBehaviour
{
    [SerializeField] Tile tile;
    [SerializeField] TMP_Text info_text;

    private void OnEnable()
    {
        if (tile != null)
        {
            UpdateUI(tile.GetTileInfo());   
        }
        
    }

    public void UpdateUI(string text)
    {
        info_text.text = text;
    }
}
