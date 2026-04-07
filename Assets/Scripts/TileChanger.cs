using UnityEngine;

public class TileChanger : MonoBehaviour
{
    public GameObject highlightObj;

    [SerializeField] private GameObject tileUI;


    public void OnClick()
    {
        Debug.Log(gameObject.name + " Says: 'Im Clicked'");

        tileUI.SetActive(true);


    }

    public void CloseUI()
    {
        tileUI.SetActive(false);

        Debug.Log(gameObject.name + " Is me and im closing my UI");

    }

    public void ChangeTile()
    {

        Debug.Log("Changing tile to ");

    }



}
