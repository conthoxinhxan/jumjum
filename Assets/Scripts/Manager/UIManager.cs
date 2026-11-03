using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject StartPanel;
    public GameObject GameOverPanel;
    public GameObject GameWinPanel;
    public GameObject HealthPanel;

    public TMP_Text CoinText;

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += HandleGameStart;
        GameManager.Instance.OnGameOver += HandleGameOver;
        GameManager.Instance.OnGameRestart += HandleGameRestart;
        GameManager.Instance.OnGetCoin += OnGetCoin;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= HandleGameStart;
        GameManager.Instance.OnGameOver -= HandleGameOver;
        GameManager.Instance.OnGameRestart -= HandleGameRestart;
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