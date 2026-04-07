using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InteractionManager : MonoBehaviour
{
    public InteractionManager Instance;

    public bool openUI;

    public UnityEvent closeUI;

    [SerializeField] private GameObject hoveredTile;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] float interactionDistance = 100f;

    private GameObject hitObj;

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
    }

    private GameObject CheckMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,interactionLayer))
        {
            //Debug.Log(hit.collider.gameObject.name);
            hitObj = hit.collider.gameObject;

            if (hitObj != null && !hitObj.GetComponent<TileChanger>().highlightObj.activeSelf)
            {
                hitObj.GetComponent<TileChanger>().highlightObj.SetActive(true);
            }

            return hitObj;

        }
        else
        {
            if (hitObj != null && hitObj.GetComponent<TileChanger>().highlightObj.activeSelf)
            {
                hitObj.GetComponent<TileChanger>().highlightObj.SetActive(false);
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
                if (hoveredTile != null)
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
                openUI = false;

                closeUI.Invoke();

                closeUI.RemoveListener(lastClickedTile.GetComponent<TileChanger>().CloseUI);

                lastClickedTile = null;

            }


        }


    }

}
