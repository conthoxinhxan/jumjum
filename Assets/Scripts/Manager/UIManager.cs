using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public HealthPanel HealthPanel;
    public PlayerItemUsePanel ItemUsePanel;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnGameStart += HandleGameStart;
        _gameManager.OnGameOver += HandleGameOver;
        _gameManager.OnGameRestart += HandleGameRestart;
    }

    private void OnDisable()
    {
        if (_gameManager == null)
        {
            return;
        }

        _gameManager.OnGameStart -= HandleGameStart;
        _gameManager.OnGameOver -= HandleGameOver;
        _gameManager.OnGameRestart -= HandleGameRestart;
        _gameManager = null;
    }

    private void HandleGameStart()
    {
    }

    private void HandleGameOver()
    {
    }

    private void HandleGameRestart()
    {
    }
}
