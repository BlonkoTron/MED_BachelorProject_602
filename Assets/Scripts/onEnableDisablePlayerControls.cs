using UnityEngine;

public class onEnableDisablePlayerControls : MonoBehaviour
{

    [SerializeField] private bool Disable = false;
    [SerializeField] private bool Enable = false;


    private void OnEnable()
    {
        if (Disable)
        {
            InteractionManager.Instance.DisablePlayerInput();
        }
        else if (Enable) 
        { 
            InteractionManager.Instance.EnablePlayerInput();
        }
        else
        {
            Debug.Log("du glemte at assign dem i inspektoren lul");
        }

    }
}
