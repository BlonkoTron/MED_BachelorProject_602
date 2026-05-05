using UnityEngine;

public class BarrenUIlort : MonoBehaviour
{
    private void OnDisable()
    {
        GameManager.Instance.SetNewState(GameManager.GameState.normal);
    }

}
