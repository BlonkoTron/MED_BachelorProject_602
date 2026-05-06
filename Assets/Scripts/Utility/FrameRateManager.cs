using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    // Set to -1 to uncap the frame rate, or enter a positive number to lock it
    [SerializeField] private int targetFrameRate = -1;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = targetFrameRate;

        Debug.Log("Target frame rate set to: " + Application.targetFrameRate);
    }
    public void ChangeTargetFrameRate(int newTarget)
    {
        Application.targetFrameRate = newTarget;
    }
}