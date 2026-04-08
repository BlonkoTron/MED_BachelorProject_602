using UnityEngine;
using UnityEngine.InputSystem;

public class Cammovement2 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 20f;
    public float edgeSize = 20f;
    public float smoothTime = 0.1f;

    [Header("Zoom")]
    public float zoomSpeed = 20f;
    public float minDistance = -60f;
    public float maxDistance = -10f;
    public float zoomSmoothTime = 0.2f;

    [Header("Map Bounds")]
    public Vector2 xLimits = new Vector2(-100, 100);
    public Vector2 zLimits = new Vector2(-100, 100);

    Vector3 moveVelocity;

    float targetDistance;
    float currentDistance;
    float zoomVelocity;

    void Start()
    {
        currentDistance = transform.position.z;
        targetDistance = currentDistance;
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    void MoveCamera()
    {
        Vector3 move = Vector3.zero;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        // Edge scrolling
        if (mousePos.x <= edgeSize)
            move.x -= 1;

        if (mousePos.x >= Screen.width - edgeSize)
            move.x += 1;

        if (mousePos.y <= edgeSize)
            move.y -= 1;

        if (mousePos.y >= Screen.height - edgeSize)
            move.y += 1;

        // WASD movement
        if (Keyboard.current.wKey.isPressed)
            move.y += 1;

        if (Keyboard.current.sKey.isPressed)
            move.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            move.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            move.x += 1;

        Vector3 target = transform.position + move * moveSpeed * Time.deltaTime;

        target.x = Mathf.Clamp(target.x, xLimits.x, xLimits.y);
        target.y = Mathf.Clamp(target.y, zLimits.x, zLimits.y);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            target,
            ref moveVelocity,
            smoothTime
        );
    }

    void ZoomCamera()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll != 0)
        {
            targetDistance -= scroll * zoomSpeed * Time.deltaTime;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        currentDistance = Mathf.SmoothDamp(
            currentDistance,
            targetDistance,
            ref zoomVelocity,
            zoomSmoothTime
        );

        Vector3 pos = transform.position;
        pos.z = currentDistance;

        transform.position = pos;
    }
}