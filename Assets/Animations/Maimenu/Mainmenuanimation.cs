using UnityEngine;

public class Mainmenuanimation : MonoBehaviour
{
    public int axis = 2;        // 0 = X, 1 = Y, 2 = Z
    public float speed = 50f;   // rotation speed

    void Update()
    {
        Vector3 rotation = Vector3.zero;

        if (axis == 0)
            rotation.x = speed;
        else if (axis == 1)
            rotation.y = speed;
        else if (axis == 2)
            rotation.z = speed;

        transform.Rotate(rotation * Time.deltaTime);
    }
}