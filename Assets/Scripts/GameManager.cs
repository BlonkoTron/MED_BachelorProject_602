using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Game Settings")]
    [SerializeField] private float tickInterval = 15f; // Time between ticks in seconds
    
    [Header("Tick Event")]
    [Tooltip("This event is invoked every tick. Subscribe tiles to this event.")]
    public UnityEvent onGameTick;
    
    private float tickTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        if (onGameTick == null)
        {
            onGameTick = new UnityEvent();
        }

        // Find all tiles in the scene and subscribe them to the tick event
        RegisterAllTiles();
    }

    void FixedUpdate()
    {
        tickTimer += Time.fixedDeltaTime;
        
        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            ProcessTick();
        }
    }
    
    private void ProcessTick()
    {
        Debug.Log("Game Tick!");
        
        // Invoke the event - all subscribed tiles will receive it
        onGameTick?.Invoke();
    }
    
    // Automatically register all tiles in the scene
    private void RegisterAllTiles()
    {
        // Clear any existing listeners to prevent duplicates
        onGameTick.RemoveAllListeners();
        
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        foreach (Tile tile in allTiles)
        {
            onGameTick.AddListener(tile.OnTick);
        }
        
        Debug.Log($"Registered {allTiles.Length} tiles to receive tick events");
    }
    
    // Call this to manually trigger a tick (useful for testing or turn-based gameplay)
    public void ManualTick()
    {
        ProcessTick();
    }
}
