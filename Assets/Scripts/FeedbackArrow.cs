using UnityEngine;

public class FeedbackArrow : MonoBehaviour
{
    [SerializeField] private float hideDelay = 3f; // Time before hiding the arrow
    
    private Animator animator;

    void OnEnable()
    {
        // Get the Animator component if not cached
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        // Play the animation (assumes there's a trigger or default animation)
        if (animator != null)
        {
            animator.SetTrigger("ArrowAnimation");
        }
        
        // Hide the arrow after the animation duration
        Invoke(nameof(HideArrow), hideDelay);
    }

    private void HideArrow()
    {
        gameObject.SetActive(false);
    }
}
