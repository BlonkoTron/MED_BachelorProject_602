using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InteractionManager : MonoBehaviour
{

    [SerializeField] private GameObject hoveredTile;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] float interactionDistance = 100f;
    
    private void Update()
    {
        hoveredTile = CheckMouseHover();
    }

    private GameObject CheckMouseHover()
    {
        GameObject hitObj;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,interactionLayer))
        {
            //Debug.Log(hit.collider.gameObject.name);
            hitObj = hit.collider.gameObject;

            return hitObj;

        }
        else
        {
            //Debug.Log("You Aint hitting shit");
            return null;
        }

        

    }




    public void OnInteract(CallbackContext action)
    {
        if (action.performed)
        {
            if (hoveredTile != null) 
            { 
                Debug.Log("Im Clickin on it: " +  hoveredTile.name);



            }
        }


    }




}
