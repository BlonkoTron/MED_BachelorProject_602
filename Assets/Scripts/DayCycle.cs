using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 3.5f;
    [SerializeField] private float xRotation = 45f;

    private float speedMultiplier = 1f;
    private GameManager gameManager;
    private float currentAngle = 0f;
    [SerializeField] private float direction = 1f;

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onGameStateChanged.AddListener(OnGameStateChanged);
        OnGameStateChanged(gameManager.gameState);
    }

    private void OnDestroy()
    {
        if (gameManager != null)
            gameManager.onGameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.normal:   speedMultiplier = 1f; break;
            case GameManager.GameState.Speedx2:  speedMultiplier = 2f; break;
            case GameManager.GameState.speedx3:  speedMultiplier = 3f; break;
            case GameManager.GameState.Paused:   speedMultiplier = 0f; break;
        }
    }

    void Update()
    {
        float delta = rotationSpeed * speedMultiplier * Time.deltaTime * direction;
        currentAngle += delta;

        transform.rotation = Quaternion.Euler(xRotation, currentAngle, 0f);
    }
}
