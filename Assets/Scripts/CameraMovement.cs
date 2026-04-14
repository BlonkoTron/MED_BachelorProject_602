using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 20f;
    public float edgeSize = 20f;
    public float smoothTime = 0.1f;

    [Header("Map Bounds")]
    public Vector2 xLimits = new Vector2(-100, 100);
    public Vector2 zLimits = new Vector2(-100, 100);

    Vector3 moveVelocity;

    void Update()
    {
        MoveCamera();
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
            move.z -= 1;

        if (mousePos.y >= Screen.height - edgeSize)
            move.z += 1;

        // WASD movement
        if (Keyboard.current.wKey.isPressed)
            move.z += 1;

        if (Keyboard.current.sKey.isPressed)
            move.z -= 1;

        if (Keyboard.current.aKey.isPressed)
            move.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            move.x += 1;

        Vector3 target = transform.position + move * moveSpeed * Time.deltaTime;

        target.x = Mathf.Clamp(target.x, xLimits.x, xLimits.y);
        target.z = Mathf.Clamp(target.z, zLimits.x, zLimits.y);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            target,
            ref moveVelocity,
            smoothTime
        );
    }
}