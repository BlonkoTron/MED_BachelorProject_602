using UnityEngine;

public class TileWarning : MonoBehaviour
{
    private Tile parentTile;
    
    
    void Start()
    {
        parentTile = GetComponentInParent<Tile>();
        parentTile.onTileChanged.AddListener(DestroyThis);   
    
    }
    
    private void DestroyThis(TileType type)
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

        parentTile.onTileChanged.RemoveListener(DestroyThis);

    }
}
