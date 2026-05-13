//Call namespaces
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;

public class GameManager : MonoBehaviour
{
    //Creates instance
    public static GameManager Instance;

    //Calls gamesettings
    public GameSettings gameSettings;
    //Calls Eventmanager
    private Eventmanager_NEWSETUP EventMangerGET;

    //Int which tracks how many ticks are left before round ends
    private int ticksTillRoundEnd;
    public int TicksTillRoundEnd => ticksTillRoundEnd;
    public int TicksBetweenRounds => gameSettings.ticksBetweenRounds;

    //Check tiles
    public Tile[] allTiles;
    public List<Tile> rainforestTiles;

    //Enable/disable tile-bools
    public bool DisableMineTilesGamesetting = false;
    public bool DisableCowTilesGamesetting = false;
    public bool DisableAgroTilesGamesetting = false;
    public bool DisableFarmTilesGamesetting = false;

    [SerializeField] private GameObject BarrenUI;
    private bool hasBarren = false;
    //Load scenes
    public enum SceneType
    {
        Lose_Happiness,
        Lose_Bio,
        Lose_Barren,
        Lose_Money,
        Win_Balance
    }

    //Load scene based on win or loss
    public void LoadScene(SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    //Call tick/roundstart/roundend Unityevent
    [Header("Tick Event")]
    [Tooltip("This event is invoked every tick. Subscribe tiles to this event.")]
    public UnityEvent onGameTick;
    public UnityEvent onRoundEnd;
    public UnityEvent onRoundStart;
    
    //Ticktimer duration
    private float tickTimer = 0f;

    //Gamestate settings
    public enum GameState { normal, Speedx2, speedx3, Paused}
    public GameState gameState = GameState.normal;
    public GameState previousGameState = GameState.normal;

    public UnityEvent<GameState> onGameStateChanged;

    //Losing condition happiness settings
    [SerializeField] private int HappinessLoseTreshold; //The threshold needed before the player can lose to happiness
    [SerializeField] private int HappinessLoseRoundThreshold;// How many rounds it should be in a row before losing
    public int HappinessTicktime = 0; //Int to count number of rounds


    //Losing condition Bioscore settings
    [SerializeField] private int RainscoreLoseTreshold; //The threshold needed before the player can lose to low bioscore
    [SerializeField] private int RainscoreLoseRoundThreshold;// How many rounds it should be in a row before losing
    public int RainscoreTicktime = 0; //Int to count number of rounds

    [SerializeField] private int MoneyLoseTreshold; //The threshold needed before the player can lose to no money

    //Winning the game settings
    [SerializeField] private int WinningThreshold; //How many events the player must survive before they can win
    private int Eventcounter; //Counter for events

    //music Get instance and refrecne to play
    private EventInstance Main_Music;
    [SerializeField] private EventReference Main_Music_MS;

    //Losing condition barren settings
    [SerializeField] private int BarrentilesLoseTreshold;
    public int BarrenTilesLoseThreshold => BarrentilesLoseTreshold;

    [Range(0f, 1f)]
    public float roundProgressValue = 0.5f;

    //Get animator
    [Header("Animation")]
    [SerializeField] private Animator clockAnimator;
    [SerializeField] private string animationName = "ClockSpinning";
    private float secondsLeftAtStart;

    private void Awake()
    {
        //Call instance
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
        hasBarren = false;
    }

    private void Start()
    {
        //Get instance
        EventMangerGET = Eventmanager_NEWSETUP.instance;
        //Play music
        Main_Music = Audiomanager.instance.PlaySound(Main_Music_MS, transform.position);
    }

    private void Update()
    {
        //Update clockui
        UpdateClockUI();
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

        //If total barren tiles is over the threshold, lose the game
        if (TileTypeAndAmountUI.Instance.barrenTiles > BarrentilesLoseTreshold)
        {
            LoseBarren();
        }
    }

    private void UpdateTickTimer()
    {
        //Updateticktimer, Change the gamespeed based on the UI
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

    //End round
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
            Lose_Warning.Instance.imagewar.gameObject.SetActive(false);
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
            Lose_Warning.Instance.imagewar.gameObject.SetActive(false);
        }

        //Money Losecheck

        if (PointSystem.Instance.CurrentMoney < MoneyLoseTreshold && TileTypeAndAmountUI.Instance.GetTotalMoneyGain() == 0)
        {
            LoseNoMoney();
        }

        //WinCondition!

        Eventcounter++;

        if (Eventcounter >= WinningThreshold && TileTypeAndAmountUI.Instance.rainforestScore > RainscoreLoseTreshold && Happiness.Instance.happinessLevel > HappinessLoseTreshold)
        {
            WinPerfectBalance();
        }
    }

    //Start round
    public void StartRound()
    {
        ticksTillRoundEnd = TicksBetweenRounds;
        SetNewState(previousGameState);
        onRoundStart.Invoke();
    }

    public void ChangeAndLockAtile(TileType type)
    {
        int randomTile = Random.Range(0,allTiles.Length);
        if (allTiles[randomTile].tileType == TileType.Rainforest || allTiles[randomTile].tileType == TileType.Grass)
        {
            allTiles[randomTile].GetComponent<TileChanger>().ChangeTileForFree(type);
            Destroy(allTiles[randomTile].gameObject.GetComponent<Collider>());
        }
        else
        {
            //Try again
            ChangeAndLockAtile(type);
        }
           
    }
    //Change+lock tile function
    public void ChangeAndLockAtile(TileType type, float degration)
    {
        int randomTile = Random.Range(0, allTiles.Length);
        if (allTiles[randomTile].tileType == TileType.Rainforest || allTiles[randomTile].tileType == TileType.Grass)
        {
            allTiles[randomTile].GetComponent<TileChanger>().ChangeTileForFree(type);
            
            if (degration > 0)
            {
                allTiles[randomTile].GetComponent<Tile>().currentDegradation = degration;
            }
            
            if (type != TileType.Grass)
            {
                Destroy(allTiles[randomTile].gameObject.GetComponent<Collider>());
            }   
        }
        else
        {
            //try again
            ChangeAndLockAtile(type,degration);
        }

    }


    // Automatically register all tiles in the scene
    private void RegisterAllTiles()
    {
        // Clear any existing listeners to prevent duplicates
        onGameTick.RemoveAllListeners();
        
        allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        foreach (Tile tile in allTiles)
        {
            onGameTick.AddListener(tile.OnTick);
        }
        
        Debug.Log($"Registered {allTiles.Length} tiles to receive tick events");
    }
    
    // Call to manually trigger a tick 
    public void ManualTick()
    {
        ProcessTick();
    }
    //Sec to round end
    public int SecondsTillRoundEnd()
    {
        int seconds = (int)gameSettings.tickInterval * ticksTillRoundEnd-(int)tickTimer;
        return seconds;
    }
    //Time till round end
    private float TimeTillRoundEnd()
    {
        float time = gameSettings.tickInterval * ticksTillRoundEnd - tickTimer;
        return time;
    }
    //Next gamestate call
    public void SetNewState(GameState state)
    {
        gameState = state;
        onGameStateChanged.Invoke(state);
    }
    //Update clock UI
    private void UpdateClockUI()
    {
        float currentVal = TimeTillRoundEnd();

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
        //Stop music and load current scene
        Audiomanager.instance.StopSound(Main_Music);
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (alt er fedt)");
        LoadScene(SceneType.Lose_Barren);
    }

    public void LoseNoBiodiversity()
    {
        //Stop music and load current scene
        Audiomanager.instance.StopSound(Main_Music);
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (no biodiversity)");
        LoadScene(SceneType.Lose_Bio);
    }

    public void LoseNoHappiness()
    {
        //Stop music and load current scene
        Audiomanager.instance.StopSound(Main_Music);
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (No money poor fool)");
        LoadScene(SceneType.Lose_Happiness);
    }

    public void LoseNoMoney()
    {
        //Stop music and load current scene
        Audiomanager.instance.StopSound(Main_Music);
        Debug.Log("YOU LOSE, YOU LOOOOOOSE (no moneypoorfool)");
        LoadScene(SceneType.Lose_Money);
    }

    public void WinPerfectBalance()
    {
        //Stop music and load current scene
        Audiomanager.instance.StopSound(Main_Music);
        Debug.Log("YOU WIN, YOU WIIIIIN (balance baby)");
        LoadScene(SceneType.Win_Balance);
    }

    public void FirstBarren()
    {
        if (!hasBarren)
        {
            SetNewState(GameState.Paused);
            hasBarren = true;
            BarrenUI.SetActive(true);

        }
    }


}
