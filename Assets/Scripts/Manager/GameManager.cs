using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int MaxHealth = 3;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    public event Action<int> OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameRestart;
    public event Action<float> OnHealthChanged;

    public event Action<int> OnGetCoin;

    private static GameManager _instance;
    private int _totalCoints;
    public float CurrentHealth { get; private set; }


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentHealth = MaxHealth;
        OnGameStart?.Invoke(MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth == 0)
            GameOver();
    }

    public void GetCoin()
    {
        _totalCoints++;
        OnGetCoin?.Invoke(_totalCoints);
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
    }
}