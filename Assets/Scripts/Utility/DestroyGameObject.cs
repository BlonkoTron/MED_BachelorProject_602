using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public void DestroyThisGameObject()
    {
        Destroy(this.gameObject);
    }
    public void DestroyThisGameObject(float time)
    {
        Destroy(this.gameObject,time);
    }
}
