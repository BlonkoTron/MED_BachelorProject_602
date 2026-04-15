using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom (0 - 1 Animation Driven)")]
    public float zoomSpeed = 5f;
    public float zoomSmoothTime = 0.2f;

    [Range(0f, 1f)]
    public float zoomValue = 0.5f;

    float targetZoom;
    float zoomVelocity;

    [Header("Animation")]
    public Animator animator;
    public string animationName = "ZoomAnimation";

    void Start()
    {
        targetZoom = zoomValue;

        if (animator != null)
        {
            animator.speed = 0f;

            // Force correct starting frame immediately
            animator.Play(animationName, 0, zoomValue);
            animator.Update(0f);
        }
    }

    void Update()
    {
        ZoomCamera();
    }

    void ZoomCamera()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll != 0)
        {
            targetZoom += scroll * zoomSpeed * 0.01f;
            targetZoom = Mathf.Clamp01(targetZoom);
        }

        zoomValue = Mathf.SmoothDamp(
            zoomValue,
            targetZoom,
            ref zoomVelocity,
            zoomSmoothTime
        );

        ApplyZoomToAnimation();
    }

    void ApplyZoomToAnimation()
    {
        if (animator == null) return;

        animator.Play(animationName, 0, zoomValue);
    }
}