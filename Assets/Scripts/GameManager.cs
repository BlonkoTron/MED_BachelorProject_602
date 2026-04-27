using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameSettings gameSettings;

    private int ticksTillRoundEnd;
    public int TicksTillRoundEnd => ticksTillRoundEnd;
    public int TicksBetweenRounds => gameSettings.ticksBetweenRounds;

    private Eventmanager_NEWSETUP EventMangerGET;

    public bool DisableMineTilesGamesetting = false;
    public bool DisableCowTilesGamesetting = false;
    public bool DisableAgroTilesGamesetting = false;
    public bool DisableFarmTilesGamesetting = false;

    public enum SceneType
    {
        Lose_Happiness,
        Lose_Bio,
        Lose_Barren,
        Win_Balance
    }

    public void LoadScene(SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }


    [Header("Tick Event")]
    [Tooltip("This event is invoked every tick. Subscribe tiles to this event.")]
    public UnityEvent onGameTick;
    public UnityEvent onRoundEnd;
    public UnityEvent onRoundStart;
    
    private float tickTimer = 0f;
    public enum GameState { normal, Speedx2, speedx3, Paused}
    public GameState gameState = GameState.normal;
    public GameState previousGameState = GameState.normal;

    public UnityEvent<GameState> onGameStateChanged;

    //Losing condition happiness settings
    [SerializeField] private int HappinessLoseTreshold; //HOw much happiness is needed to be under threshold
    [SerializeField] private int HappinessLoseRoundThreshold;// How many rounds it should be in a row befor elosing
    public int HappinessTicktime = 0; //Int tto count number of rounds


    //Losing condition happiness settings
    [SerializeField] private int RainscoreLoseTreshold; //HOw much happiness is needed to be under threshold
    [SerializeField] private int RainscoreLoseRoundThreshold;// How many rounds it should be in a row befor elosing
    public int RainscoreTicktime = 0; //Int tto count number of rounds

    //Losing condition barren settings
    [SerializeField] private int BarrentilesLoseTreshold;

    //Winning the game settings

    [SerializeField] private int WinningThreshold; //How many events the player must survive before they can win
    private int Eventcounter; //Counter for events

    [Range(0f, 1f)]
    public float roundProgressValue = 0.5f;

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
        ticksTillRoundEnd = TicksBetweenRounds;
        secondsLeftAtStart = SecondsTillRoundEnd();
        // Find all tiles in the scene and subscribe them to the tick event
        RegisterAllTiles();

        //Call eventmanager
    }
    private void Start()
    {
        EventMangerGET = Eventmanager_NEWSETUP.instance;
    }

    void FixedUpdate()
    {
        UpdateTickTimer();
        if (tickTimer >= gameSettings.tickInterval)
        {
            tickTimer = 0f;
            ProcessTick();
        }

        //Lose checkmarks

        //All barren
        if (TileTypeAndAmountUI.Instance.barrenTiles > BarrentilesLoseTreshold)
        {
            LoseBarren();
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
        ticksTillRoundEnd = TicksBetweenRounds;
        onRoundEnd?.Invoke();
        previousGameState = gameState;
        SetNewState(GameState.Paused);
        //Call event here
        EventMangerGET.TriggerRandomEvent();
        
        //HappyLosecheck

        if (Happiness.Instance.happinessLevel < HappinessLoseTreshold)
        {
            HappinessTicktime++;
            Lose_Warning.Instance.Warning_happy = true;

            if (HappinessTicktime == HappinessLoseRoundThreshold)
            {
                LoseNoHappiness();
            }
        }
        else if (Happiness.Instance.happinessLevel >= HappinessLoseTreshold)
        {
            HappinessTicktime = 0;
            Lose_Warning.Instance.Warning_happy = false;
        }

        //Rainforestscore Losecheck

        if (TileTypeAndAmountUI.Instance.rainforestScore < RainscoreLoseTreshold)
        {
            RainscoreTicktime++;
            Lose_Warning.Instance.Warning_bio = true;

            if (RainscoreTicktime == RainscoreLoseRoundThreshold)
            {
                LoseNoBiodiversity();
            }
        }
        else if (TileTypeAndAmountUI.Instance.rainforestScore >= RainscoreLoseTreshold)
        {
            RainscoreTicktime = 0;
            Lose_Warning.Instance.Warning_bio = false;
        }

        //WinCondition!

        Eventcounter++;

        if (Eventcounter >= WinningThreshold && TileTypeAndAmountUI.Instance.rainforestScore > RainscoreLoseTreshold && Happiness.Instance.happinessLevel > HappinessLoseTreshold)
        {
            WinPerfectBalance();
        }
    }

    public void StartRound()
    {
        ticksTillRoundEnd = TicksBetweenRounds;
        SetNewState(previousGameState);
        onRoundStart.Invoke();
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
        int seconds = (int)gameSettings.tickInterval * ticksTillRoundEnd-(int)tickTimer;
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
        roundProgressValue = progress;

    }


    //Losing conditions
    public void LoseBarren()
    {
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (alt er fedt)");
        LoadScene(SceneType.Lose_Barren);
    }

    public void LoseNoBiodiversity()
    {
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (no biodiversity)");
        LoadScene(SceneType.Lose_Bio);
    }

    public void LoseNoHappiness()
    {
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (no happy)");
        LoadScene(SceneType.Lose_Happiness);
    }

    public void WinPerfectBalance()
    {
        Debug.Log("YOU WIN, YOU WIIIIIN (balance baby)");
        LoadScene(SceneType.Win_Balance);
    }

}
