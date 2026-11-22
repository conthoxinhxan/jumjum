using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameLoader _loader;
    private static GameManager _instance;

    public static GameManager Instance => _instance;
    public PlayerController CurrentPlayer { get; private set; }
    public UIManager UIManager;
    public CameraControl playerCamera;

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameRestart;
    public event Action<PlayerController> OnPlayerChanged;
    public event Action<float> OnLoadingProgressChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _loader = GameLoader.Instance;
        _loader.OnLoadingProgressChanged += HandleLoadingProgress;
        _loader.RetryRequested += RestartGame;
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        _loader.OnLoadingProgressChanged -= HandleLoadingProgress;
        _loader.RetryRequested -= RestartGame;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentPlayer = _loader.LoadPlayer();
        StartSession(CurrentPlayer);
    }

    public void RestartGame()
    {
    }

    private void HandleLoadingProgress(float progress)
    {
        OnLoadingProgressChanged?.Invoke(progress);
    }

    private void StartSession(PlayerController player)
    {
        var playerHealth = CurrentPlayer.GetComponent<PlayerHealth>();
        UIManager.HealthPanel.Initialize(playerHealth);
        playerHealth.Initialize();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnPlayerChanged(CurrentPlayer);
    }

    public void GameOver()
    {
        
    }

    private void HandlePlayerDied()
    {
        GameOver();
    }
}