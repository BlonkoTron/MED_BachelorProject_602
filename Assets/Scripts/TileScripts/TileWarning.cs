using UnityEngine;

public class TileWarning : MonoBehaviour
{
    [SerializeField] private float animationDuration = 1.5f; // Duration before self-destruct
    private Tile parentTile;
    private Animator animator;
    
    void Start()
    {
        // Get reference to parent tile
        parentTile = GetComponentInParent<Tile>();
        
        // Get animator if present
        animator = GetComponent<Animator>();
        
        // If animator exists, play the warning animation
        if (animator != null)
        {
            animator.SetTrigger("PlayWarning");
        }
        
        // Destroy THIS warning object (not the tile) after animation duration
        Destroy(gameObject, animationDuration);
    }
    
    void OnDestroy()
    {
        // Reset the warning flag on the parent tile
        if (parentTile != null)
        {
            parentTile.ResetWarningFlag();
        }
    }
}
