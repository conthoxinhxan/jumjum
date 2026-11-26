using System;
using System.Threading.Tasks;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    private const string PlayerAddress = "Assets/Prefabs/Player.prefab";
    private readonly AddressableLoader _addressableLoader = new AddressableLoader();
    private GameObject _playerInstance;

    public static GameLoader Instance { get; private set; }

    public bool IsLoading { get; private set; }
    public float LoadingProgress { get; private set; }
    public string LoadingError { get; private set; }

    public event Action<float> OnLoadingProgressChanged;
    internal event Action RetryRequested;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureInstance()
    {
        if (Instance == null)
            new GameObject(nameof(GameLoader)).AddComponent<GameLoader>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
            
    }

    // internal void BeginLoading(string scenePath, Action onStarted, Action<PlayerController> onCompleted)
    // {
    //     if (IsLoading)
    //     {
    //         return;
    //     }
           
    //     IsLoading = true;
    //     LoadingError = null;
    //     onStarted();
    //     SetLoadingProgress(0f);
    // }


    // private void SetLoadingProgress(float progress)
    // {
    //     LoadingProgress = Mathf.Clamp01(progress);
    //     OnLoadingProgressChanged?.Invoke(LoadingProgress);
    // }

    // private void FailLoading(string error)
    // {
    //     LoadingError = error;
    //     IsLoading = false;
    //     Debug.LogError(error, this);
    // }

    public async Task<PlayerController> LoadPlayerAsync()
    {
        _playerInstance = await _addressableLoader.InstantiateAsync(PlayerAddress);
        var player = _playerInstance.GetComponent<PlayerController>();

        if (player == null)
        {
            _addressableLoader.ReleaseInstance(_playerInstance);
            _playerInstance = null;
            throw new InvalidOperationException($"Addressable player '{PlayerAddress}' has no PlayerController component.");
        }

        return player;
    }
}
