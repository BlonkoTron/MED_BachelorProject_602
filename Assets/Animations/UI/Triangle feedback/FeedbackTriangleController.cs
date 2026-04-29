using UnityEngine;
//Ai'd big time
public class FeedbackTriangleController : MonoBehaviour
{
    public Animator animator;

    public void TriggerChange(int delta)
    {
        gameObject.SetActive(true);
        if (delta > 0) animator.SetTrigger("FeedbackGreenBlink");
        else if (delta < 0) animator.SetTrigger("FeedbackRedBlink");
    }

    public void OnAnimationFinished()
    {
        // Don't hide if the hover 'On' states are active
        if (!animator.GetBool("FeedbackRedOn") && !animator.GetBool("FeedbackGreenOn"))
            gameObject.SetActive(false);
    }
}