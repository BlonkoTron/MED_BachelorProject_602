using UnityEngine;

public class TileChanger : MonoBehaviour
{
    public GameObject highlightObj;

    [SerializeField] private GameObject tileUI;

    private Tile tile;

    private void Start()
    {
        if (tileUI != null && tileUI.activeInHierarchy != true)
        {
            tileUI.SetActive(false);
        }

        tile = GetComponent<Tile>();
        
        
    }

    public void OnClick()
    {
        //Debug.Log(gameObject.name + " Says: 'Im Clicked'");

        tileUI.SetActive(true);


    }

    public void CloseUI()
    {

        tileUI.GetComponent<Animator>().SetTrigger("Reset");

        tileUI.SetActive(false);

        //Debug.Log(gameObject.name + " Is me and im closing my UI");

    }

    public void ChangeTile(TileType type)
    {
        Debug.Log("Changing tile to " + type);

        tile.SetTileType(type);

        CloseUI();

    }



}
