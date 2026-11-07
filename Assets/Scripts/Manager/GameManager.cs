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

    public event Action<int> OnGetCoin;

    private static GameManager _instance;
    private int _totalCoints;


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
        OnGameStart?.Invoke(MaxHealth);
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