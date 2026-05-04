using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class FactBoxTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 5)]
    [Tooltip("The text that will appear in the fact box.")]
    public string factBoxContent;

    [Tooltip("Delay in seconds before the box appears.")]
    public float delay = 0.3f;

    private Coroutine delayCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        delayCoroutine = StartCoroutine(ShowWithDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (delayCoroutine != null) StopCoroutine(delayCoroutine);
        FactBoxController.Instance.HideFactBox();
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(delay);
        // Call the manager without passing the mouse position (manager now handles reading the position)
        FactBoxController.Instance.ShowFactBox(factBoxContent);
    }
}