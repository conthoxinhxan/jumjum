using UnityEngine;

[DefaultExecutionOrder(10000)]
public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private float _showDistance = 3f;
    [SerializeField] private GameObject _panel;

    private GameManager _gameManager;
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        BindGameManager();
    }

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        BindGameManager();
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.OnPlayerChanged -= HandlePlayerChanged;
            _gameManager = null;
        }
    }

    private void BindGameManager()
    {
        GameManager manager = GameManager.Instance;
        if (_gameManager == manager)
        {
            return;
        }

        if (_gameManager != null)
        {
            _gameManager.OnPlayerChanged -= HandlePlayerChanged;
        }

        _gameManager = manager;
        if (_gameManager != null)
        {
            _gameManager.OnPlayerChanged += HandlePlayerChanged;
            HandlePlayerChanged(_gameManager.CurrentPlayer);
        }
    }

    private void HandlePlayerChanged(PlayerController player)
    {
        _player = player != null ? player.transform : null;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (_camera == null || _player == null)
        {
            return;
        }

        float distance = Vector3.Distance(_player.position, transform.position);

        bool isNearby = distance <= _showDistance;

        if (_panel != null)
        {
            _panel.SetActive(isNearby);
        }
        else if (_canvas != null)
        {
            _canvas.enabled = isNearby;
        }

        if (isNearby)
        {
            Vector3 directionToCamera = _camera.transform.position - transform.position;
            if (directionToCamera.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(directionToCamera, _camera.transform.up);
            }
        }
    }
}