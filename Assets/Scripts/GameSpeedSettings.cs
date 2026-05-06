using UnityEngine;
using UnityEngine.InputSystem;
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
        gameManager.onRoundEnd.AddListener(Disablebuttons);
        gameManager.onRoundStart.AddListener(EnableButtons);
        UpdateSpeedSettingButtons(gameManager.gameState);
    }
    private void OnDestroy()
    {
        gameManager.onGameStateChanged.RemoveListener(UpdateSpeedSettingButtons);
        gameManager.onRoundEnd.RemoveListener(Disablebuttons);
        gameManager.onRoundStart.RemoveListener(EnableButtons);
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

    //Spacebar PUASE HERE
    private void Update()
    {

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {

            if (TutorialManager.instance == null)
            {
                TogglePauseAndNormal();
            }
        }
    }

    private void TogglePauseAndNormal()
    {
        if (gameManager.gameState == GameManager.GameState.Paused)
        {
            SetGameStateNormal(); 
        }
        else
        {
            SetGameStatePaused(); 
        }
    }

    //Spacebar PUASE End HERE

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
    private void Disablebuttons()
    {
        normalSpeedButton.enabled = false;
        x2SpeedButton.enabled = false;
        x3SpeedButton.enabled = false;
        pauseButton.enabled = false;
    }
    private void EnableButtons()
    {
        normalSpeedButton.enabled = true;
        x2SpeedButton.enabled = true;
        x3SpeedButton.enabled = true;
        pauseButton.enabled = true;
    }

}
