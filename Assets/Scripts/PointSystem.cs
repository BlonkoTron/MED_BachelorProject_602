using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [Header("Money Tracking")]
    [SerializeField] private int currentMoney = 0;
    
    public int CurrentMoney => currentMoney;
    
    void Start()
    {
        // Find all tiles and subscribe to their money events
        RegisterAllTiles();
    }
    
    private void RegisterAllTiles()
    {
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        foreach (Tile tile in allTiles)
        {
            // Remove listener first to prevent duplicates
            tile.onMoneyEarned.RemoveListener(AddMoney);
            tile.onMoneyEarned.AddListener(AddMoney);
        }
        
        Debug.Log($"PointSystem registered {allTiles.Length} tiles for money tracking");
    }
    
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log($"Earned ${amount}. Total money: ${currentMoney}");
    }
    
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log($"Spent ${amount}. Remaining money: ${currentMoney}");
            return true;
        }
        else
        {
            Debug.Log($"Not enough money! Need ${amount}, have ${currentMoney}");
            return false;
        }
    }
    
    void OnDestroy()
    {
        // Unsubscribe from all tiles
        Tile[] allTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        
        foreach (Tile tile in allTiles)
        {
            tile.onMoneyEarned.RemoveListener(AddMoney);
        }
    }
}
