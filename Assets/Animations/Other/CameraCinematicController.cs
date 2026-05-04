using UnityEngine;

public class CameraCinematicController : MonoBehaviour
{
    public enum CameraMode { FreeOrbit, MarkerTransition }

    [Header("Mode Selection")]
    public CameraMode currentMode = CameraMode.FreeOrbit;

    [Header("Mode 1: Free Orbit Settings")]
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 2f;
    public Vector3 constantMoveDirection = Vector3.zero;
    public float moveSpeed = 0f;

    [Header("Mode 2: Marker Transition Settings")]
    public Transform pointA;
    public Transform pointB;
    [Range(0, 1)] public float transitionProgress = 0f;
    public float transitionSpeed = 0.5f;
    public bool autoPlayTransition = true;
    private bool movingToB = true;

    void Update()
    {
        if (currentMode == CameraMode.FreeOrbit)
        {
            HandleFreeOrbit();
        }
        else
        {
            HandleMarkerTransition();
        }
    }

    private void HandleFreeOrbit()
    {
        // Continuous Rotation
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.World);

        // Optional Continuous Movement
        if (moveSpeed > 0)
        {
            transform.Translate(constantMoveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void HandleMarkerTransition()
    {
        if (pointA == null || pointB == null) return;

        // Auto-ping-pong logic
        if (autoPlayTransition)
        {
            float step = transitionSpeed * Time.deltaTime;
            transitionProgress += movingToB ? step : -step;

            if (transitionProgress >= 1f) movingToB = false;
            if (transitionProgress <= 0f) movingToB = true;
        }

        // Smoothly interpolate Position and Rotation
        transform.position = Vector3.Lerp(pointA.position, pointB.position, transitionProgress);
        transform.rotation = Quaternion.Lerp(pointA.rotation, pointB.rotation, transitionProgress);
    }

    [ContextMenu("Snap to Point A")]
    public void SnapToA() => SetToMarker(pointA);

    [ContextMenu("Snap to Point B")]
    public void SnapToB() => SetToMarker(pointB);

    private void SetToMarker(Transform target)
    {
        if (target == null) return;
        transform.position = target.position;
        transform.rotation = target.rotation;
        transitionProgress = (target == pointA) ? 0f : 1f;
    }
}