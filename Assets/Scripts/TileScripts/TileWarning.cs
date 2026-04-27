using UnityEngine;

public class TileWarning : MonoBehaviour
{
    private Tile parentTile;
    
    
    void Start()
    {

        GameManager.Instance.onGameTick.AddListener(DestoryThis);   
    
    }
    
    private void DestoryThis()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Reset the warning flag on the parent tile
        if (parentTile != null)
        {
            parentTile.ResetWarningFlag();
        }

        GameManager.Instance.onGameTick.RemoveListener(DestoryThis);

    }
}
