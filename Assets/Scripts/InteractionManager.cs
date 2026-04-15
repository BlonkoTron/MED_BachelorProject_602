using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    public bool openUI;

    public UnityEvent closeUI;

    [SerializeField] private GameObject hoveredTile;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] LayerMask UILayer;
    [SerializeField] float interactionDistance = 100f;

    private bool hoveringUI;

    private GameObject hitObj;
    private GameObject lastHoveredTile;
    private GameObject lastClickedTile;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        hoveredTile = CheckMouseHover();
        hoveringUI = CheckIfUI();
    }

    private GameObject CheckMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            //Debug.Log(hit.collider.gameObject.name);
            hitObj = hit.collider.gameObject;

            if (hitObj.GetComponent<TileChanger>() != null)
            {
                if (hitObj != null && !hitObj.GetComponent<TileChanger>().highlightObj.activeSelf)
                {

                    if (lastHoveredTile != null && lastHoveredTile != hitObj.GetComponent<TileChanger>().highlightObj)
                    {
                        lastHoveredTile.SetActive(false);
                        lastHoveredTile = null;
                    }

                    hitObj.GetComponent<TileChanger>().highlightObj.SetActive(true);
                    lastHoveredTile = hitObj.GetComponent<TileChanger>().highlightObj;
                }
            }
            return hitObj;

        }
        else
        {
            if (lastHoveredTile != null)
            {
                lastHoveredTile.SetActive(false);
                lastHoveredTile = null;
            }

            return null;
        }

        

    }




    public void OnInteract(CallbackContext action)
    {
        if (action.performed)
        {
            if (!openUI)
            {
                if (hoveredTile != null && hoveredTile.GetComponent<Tile>().Type != TileType.Barren)
                {
                    Debug.Log("Im Clickin on it: " + hoveredTile.name);

                    hoveredTile.GetComponent<TileChanger>().OnClick();

                    closeUI.AddListener(hoveredTile.GetComponent<TileChanger>().CloseUI);

                    lastClickedTile = hoveredTile;
                    openUI = true;

                }
            }
            else
            {

                if (hoveringUI)
                {
                    Debug.Log("Hit the Button");
                    openUI = false;

                    if (lastClickedTile != null)
                    {
                        closeUI.RemoveListener(lastClickedTile.GetComponent<TileChanger>().CloseUI);
                        lastClickedTile = null;
                    }
                }
                else
                {
                    // Clicked on empty space (not UI)
                    openUI = false;

                    closeUI.Invoke();

                    if (lastClickedTile != null)
                    {
                        closeUI.RemoveListener(lastClickedTile.GetComponent<TileChanger>().CloseUI);
                        lastClickedTile = null;
                    }

                }

            }


        }


    }

    private bool CheckIfUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else { return false; }
    }

    private void OnDestroy()
    {
        if (lastClickedTile != null)
        {
            closeUI.RemoveListener(lastClickedTile.GetComponent<TileChanger>().CloseUI);
        }
    }

}
