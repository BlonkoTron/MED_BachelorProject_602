using UnityEngine;
using UnityEngine.UI;

public class GameSpeedSettings : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private Button normalSpeedButton;
    [SerializeField] private Button x2SpeedButton;
    [SerializeField] private Button x3SpeedButton;
    [SerializeField] private Button pauseButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onGameStateChanged.AddListener(UpdateSpeedSettingButtons);
        UpdateSpeedSettingButtons(gameManager.gameState);
    }

    private void UpdateSpeedSettingButtons(GameManager.GameState state)
    {
        switch(state)
        {
            case GameManager.GameState.normal:
                normalSpeedButton.interactable = false;
                x2SpeedButton.interactable = true;
                x3SpeedButton.interactable = true;
                pauseButton.interactable = true;
                break;
            case GameManager.GameState.Paused:
                normalSpeedButton.interactable = true;
                x2SpeedButton.interactable = true;
                x3SpeedButton.interactable = true;
                pauseButton.interactable = false;
                break;
            case GameManager.GameState.Speedx2:
                normalSpeedButton.interactable = true;
                x2SpeedButton.interactable = false;
                x3SpeedButton.interactable = true;
                pauseButton.interactable = true;
                break;
            case GameManager.GameState.speedx3:
                normalSpeedButton.interactable = true;
                x2SpeedButton.interactable = true;
                x3SpeedButton.interactable = false;
                pauseButton.interactable = true;
                break;
        }
    }
    public void SetGameStateNormal()
    {
        gameManager.SetNewState(GameManager.GameState.normal);
    }
    public void SetGameStatePaused()
    {
        gameManager.SetNewState(GameManager.GameState.Paused);
    }
    public void SetGameState2XSpeed()
    {
        gameManager.SetNewState(GameManager.GameState.Speedx2);
    }
    public void SetGameState3XSpeed()
    {
        gameManager.SetNewState(GameManager.GameState.speedx3);
    }

}
