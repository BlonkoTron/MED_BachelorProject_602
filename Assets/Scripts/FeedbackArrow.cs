using UnityEngine;

public class FeedbackArrow : MonoBehaviour
{
    [SerializeField] private float hideDelay = 3f; // Time before hiding the arrow
    [SerializeField] private string positiveAnimationTrigger = "PositiveTrigger";
    [SerializeField] private string negativeAnimationTrigger = "NegativeTrigger";
    
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayPositiveAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(positiveAnimationTrigger);
        }
        
        // Cancel any previous hide invokes
        CancelInvoke(nameof(HideArrow));
        // Hide the arrow after the animation duration
        Invoke(nameof(HideArrow), hideDelay);
    }

    public void PlayNegativeAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(negativeAnimationTrigger);
        }
        
        // Cancel any previous hide invokes
        CancelInvoke(nameof(HideArrow));
        // Hide the arrow after the animation duration
        Invoke(nameof(HideArrow), hideDelay);
    }

    private void HideArrow()
    {
        gameObject.SetActive(false);
    }
}
