using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Game Settings")]
    [SerializeField] private float tickInterval = 15f; // Time between ticks in seconds
    [SerializeField] private int ticksBetweenRounds = 5;
    private int ticksTillRoundEnd;
    public int TicksTillRoundEnd => ticksTillRoundEnd;
    public int TicksBetweenRounds => ticksBetweenRounds;

    //Get eventmanager
    [SerializeField] private Eventmanager_NEWSETUP EventMangerGET;

    [Header("Tick Event")]
    [Tooltip("This event is invoked every tick. Subscribe tiles to this event.")]
    public UnityEvent onGameTick;
    public UnityEvent onRoundEnd;
    
    private float tickTimer = 0f;
    public enum GameState { normal, Speedx2, speedx3, Paused}
    public GameState gameState = GameState.normal;

    public UnityEvent<GameState> onGameStateChanged;

    [Range(0f, 1f)]
    public float clockSpinValue = 0.5f;

    [Header("Animation")]
    [SerializeField] private Animator clockAnimator;
    [SerializeField] private string animationName = "ClockSpinning";
    private float secondsLeftAtStart;

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
        ticksTillRoundEnd = ticksBetweenRounds;
        secondsLeftAtStart = SecondsTillRoundEnd();
        // Find all tiles in the scene and subscribe them to the tick event
        RegisterAllTiles();

        //Call eventmanager
        EventMangerGET = Eventmanager_NEWSETUP.instance;
    }

    void FixedUpdate()
    {
        UpdateTickTimer();
        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            ProcessTick();
        }
    }
    private void UpdateTickTimer()
    {

        UpdateClockUI();

        switch (gameState)
        {
            case GameState.normal:
                tickTimer += Time.fixedDeltaTime;
                break;
            case GameState.Speedx2:
                tickTimer += Time.fixedDeltaTime*2;
                break;
            case GameState.speedx3:
                tickTimer += Time.fixedDeltaTime*3;
                break;
            case GameState.Paused:
                break;
            default:
               tickTimer += Time.fixedDeltaTime;
                break;
        }

        

    }
    
    private void ProcessTick()
    {
        Debug.Log("Game Tick!");
        // Invoke the event - all subscribed tiles will receive it
        onGameTick?.Invoke();
        ticksTillRoundEnd--;
        if (ticksTillRoundEnd<=0)
        {
            EndRound();
        }
    }
    private void EndRound()
    {
        ticksTillRoundEnd = ticksBetweenRounds;
        onRoundEnd?.Invoke();
        SetNewState(GameState.Paused);
        //Call event here
        EventMangerGET.TriggerRandomEvent();
    }
    public void StartRound()
    {
        ticksTillRoundEnd = ticksBetweenRounds;
        SetNewState(GameState.normal);
    }

    // Automatically register all tiles in the scene
    private void RegisterAllTiles()
    {
        // when generating the world we should already know where the tiles are, removing the need to find them.
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
    public int SecondsTillRoundEnd()
    {
        int seconds = (int)tickInterval * ticksTillRoundEnd-(int)tickTimer;
        return seconds;
    }
    public void SetNewState(GameState state)
    {
        gameState = state;
        onGameStateChanged.Invoke(state);
    }

    private void UpdateClockUI()
    {
        float currentVal = SecondsTillRoundEnd();

        float progress = 1.0f - (Mathf.Clamp(currentVal, 0, secondsLeftAtStart) / secondsLeftAtStart);

        if (clockAnimator != null) 
        {
            clockAnimator.Play(animationName, 0, progress);
        }
        

    }

}
