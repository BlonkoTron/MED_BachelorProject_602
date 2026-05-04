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
    private PlayerInput playerInput;


    private GameObject hitObj;
    private GameObject lastHoveredTile;
    private GameObject lastClickedTile;


    Camera cam;
    Vector3 newPosition;
    [SerializeField] private float movementTime = 5f;
    Vector3 dragStartPosition = Vector3.zero;
    Vector3 dragCurrentPosition = Vector3.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerInput = GetComponent<PlayerInput>();

        newPosition = transform.position;
        cam = Camera.main;

    }

    private void Update()
    {
        hoveredTile = CheckMouseHover();
        hoveringUI = CheckIfUI();

        ApplyMovements();

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

    private void ApplyMovements()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, newPosition, movementTime * Time.deltaTime);
    }


    public void OnInteract(CallbackContext action)
    {

        
        System.Type vector2Type = Vector2.zero.GetType();
        if (action.started)
        {
            Debug.Log("Button Pressed Down Event - called once when button pressed");

            Ray dragStartRay = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane dragStartPlane = new Plane(Vector3.up, Vector3.zero);
            float dragStartEntry;

            if (dragStartPlane.Raycast(dragStartRay, out dragStartEntry))
            {
                dragStartPosition = dragStartRay.GetPoint(dragStartEntry);
            }

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
                        lastClickedTile = null;
                    }

                }

            }


        }
        else if (action.performed)
        {
            
            Debug.Log("Button Hold Down - called continously till the button is pressed");

            Ray dragCurrentRay = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane dragCurrentPlane = new Plane(Vector3.up, Vector3.zero);
            float dragCurrentEntry;

            if (dragCurrentPlane.Raycast(dragCurrentRay, out dragCurrentEntry))
            {
                dragCurrentPosition = dragCurrentRay.GetPoint(dragCurrentEntry);
                newPosition = cam.transform.position + dragStartPosition - dragCurrentPosition;
            }

        }


    }

    public void DisablePlayerInput()
    {
        if (playerInput.enabled)
        {
            playerInput.enabled = false;
        }

    }

    public void EnablePlayerInput()
    {
        if (!playerInput.enabled)
        {
            playerInput.enabled = true;
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
