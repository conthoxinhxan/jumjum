using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public GameObject HealthPanel;

    public TMP_Text CoinText;

    private GameManager _gameManager;

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnGameStart += HandleGameStart;
        _gameManager.OnGameOver += HandleGameOver;
        _gameManager.OnGameRestart += HandleGameRestart;
        _gameManager.OnGetCoin += OnGetCoin;
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
        _gameManager.OnGetCoin -= OnGetCoin;
        _gameManager = null;
    }

    private void HandleGameStart(int maxHealth)
    {
        // StartPanel.SetActive(false);
        // GameOverPanel.SetActive(false);
        // GameWinPanel.SetActive(false);
        HealthPanel.SetActive(true);
        HealthPanel.GetComponent<HealthPanel>().Initialize(maxHealth);
    }

    private void HandleGameOver()
    {
        // StartPanel.SetActive(false);
        // GameOverPanel.SetActive(true);
        // GameWinPanel.SetActive(false);
    }

    private void HandleGameRestart()
    {
        // StartPanel.SetActive(true);
        // GameOverPanel.SetActive(false);
        // GameWinPanel.SetActive(false);
        HealthPanel.SetActive(false);
    }

    private void OnGetCoin(int totalCoins)
    {
        CoinText.text = "Coins: " + totalCoins.ToString();
    }
}