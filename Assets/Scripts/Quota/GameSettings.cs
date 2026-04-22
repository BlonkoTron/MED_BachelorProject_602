using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public int STARTING_MONEY = 0;

    [Header("Game Speed")]
    [Tooltip("Time between ticks in seconds")]
    public float tickInterval = 15f;
    public int ticksBetweenRounds = 5;

    [Header("Degradation Rates (% per tick)")]
    // Degradation Rate Constants
    public float DEGRADATION_MINE = 50f;
    public float DEGRADATION_COW_FIELD = 34f;
    public float DEGRADATION_FARM = 25f;
    public float DEGRADATION_AGROFOREST = 10f;
    public float DEGRADATION_NATURAL_REGEN = -10f;
    public float DEGRADATION_NONE = 0f;

    [Header("Income (per tick)")]
    // Money Per Tick Constants
    public int MONEY_MINE = 100;
    public int MONEY_COW_FIELD = 60;
    public int MONEY_FARM = 50;
    public int MONEY_AGROFOREST = 30;
    public int MONEY_NATURAL = 0;

    [Header("Build Cost")]
    // Money Per Tick Constants
    public int COST_MINE = 0;
    public int COST_COW_FIELD = 0;
    public int COST_FARM = 0;
    public int COST_AGROFOREST = 0;
    public int COST_NATURAL = 0;

    [Header("Happiness")]
    [Tooltip("Happiness penalty when changing a rainforest tile to another type")]
    public int HAPPINESS_PENALTY_RAINFOREST_CHANGE = 2;

    [Header("Quota")]
    public int[] quotaAmounts= { 10,20,40,60,100};

    [Header("Tile Lock")]
    public bool DisableMineTilesGamesetting = false;
    public bool DisableCowTilesGamesetting = false;
    public bool DisableAgroTilesGamesetting = false;
    public bool DisableFarmTilesGamesetting = false;



}
