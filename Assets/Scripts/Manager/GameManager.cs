using System;
using System.IO;
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
        var playerData = LoadGame();

        if (playerData == null)
        {
            // its means create new game
            // for now just create a start level first
            playerData = new PlayerData();
            playerData.CurrentHealth = 20;
            playerData.MaxHeath = 50;
            playerData.DateTime = DateTime.Now.ToString();
            playerData.Inventory = new PlayerInventoryData();
            
            playerData.Position = new Position();
            playerData.Position.X = 0;
            playerData.Position.Y = 0;
            playerData.Position.Z = 0;
        }

        CurrentPlayer.Initialize(playerData);
        UIManager.HealthPanel.Initialize(player.Health);
        UIManager.UseItemPanel?.Initialize(player.ItemUse);

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

    public void SaveGame()
    {
        PlayerData data = new PlayerData();
        data.Name = "Player";
        data.DateTime = DateTime.Now.ToString();
        data.CurrentHealth = CurrentPlayer.Health.CurrentHealth;
        data.MaxHeath = CurrentPlayer.Health.MaxHealth;
        data.Inventory = new PlayerInventoryData 
        { 
            Items = CurrentPlayer.ItemUse.Inventory
        };
        
        Position pos = new Position();
        pos.X = CurrentPlayer.transform.position.x;
        pos.Y = CurrentPlayer.transform.position.y;
        pos.Z = CurrentPlayer.transform.position.z;

        data.Position = pos;

        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

    }

    public PlayerData LoadGame()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (!File.Exists(savePath))
        {
            return null;
        }

        string json = File.ReadAllText(savePath);
        var playerData = JsonUtility.FromJson<PlayerData>(json);
        return playerData;
    }
}
