using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlidePanel : MonoBehaviour
{
    public RectTransform panel;        // The UI panel to move
    public RectTransform buttonIcon;   // The button (or arrow icon) to rotate

    public Vector2 shownPosition;      // Where the panel is visible
    public Vector2 hiddenPosition;     // Where the panel is hidden

    public float duration = 0.3f;

    private bool isOpen = true;
    private Coroutine currentRoutine;

    public void TogglePanel()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        isOpen = !isOpen;

        Vector2 startPos = panel.anchoredPosition;
        Vector2 targetPos = isOpen ? shownPosition : hiddenPosition;

        float startRot = isOpen ? 0f : 180f;
        float targetRot = isOpen ? 180f : 0f;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            float rot = Mathf.LerpAngle(startRot, targetRot, t);
            buttonIcon.localEulerAngles = new Vector3(0, 0, rot);

            time += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPos;
        buttonIcon.localEulerAngles = new Vector3(0, 0, targetRot);
    }
}