using UnityEngine;

public class TileButtons : MonoBehaviour
{
    [SerializeField] private TileChanger ParentTile;

    [SerializeField] private TileType tileType;


    public void OnClick()
    {
        ParentTile.ChangeTile(tileType);
    }


}
