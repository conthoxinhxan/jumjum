using UnityEngine;

[ExecuteAlways]
public class CameraControl : MonoBehaviour
{
    public Transform Target;

    [Header("Camera Offset")]
    public Vector3 CameraOffsetPosition = new Vector3(0f, 1.6f, -4f);

    [Header("Mouse")]
    public float MouseSensitivity = 3f;
    public float MinPitch = -40f;
    public float MaxPitch = 60f;

    private float _mouseX;
    private float _mouseY;
    private PlayerInput _playerInput;
    private GameManager _gameManager;

    private void Start()
    {
        if (!Application.isPlaying)
            return;

        _gameManager = GameManager.Instance;
        _gameManager.OnPlayerChanged += HandlePlayerChanged;
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.OnPlayerChanged -= HandlePlayerChanged;
        }
           
    }

    private void HandlePlayerChanged(PlayerController player)
    {
        SetTarget(player != null ? player.transform : null);
    }

    private void SetTarget(Transform target)
    {
        Target = target;
        _playerInput = target != null ? target.GetComponent<PlayerInput>() : null;
        _mouseX = target != null ? target.eulerAngles.y : 0f;
        _mouseY = 0f;
    }

    private void Update()
    {
        if (!Application.isPlaying || Target == null || _playerInput == null)
        {
            return;
        }

        Vector2 lookInput = _playerInput.LookInput;
        _mouseX += lookInput.x * MouseSensitivity;
        _mouseY -= lookInput.y * MouseSensitivity;

        // Giới hạn giá trị _mouseY để tránh camera quay quá cao hoặc quá thấp.
        _mouseY = Mathf.Clamp(_mouseY, MinPitch, MaxPitch);

        // Player chỉ quay ngang quanh trục Y.
        Target.rotation = Quaternion.Euler(0f, _mouseX, 0f);
    }

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        Quaternion cameraRotation = Quaternion.Euler(_mouseY, _mouseX, 0f);

        // Xoay offset theo hướng nhìn rồi cộng vào vị trí Target.
        transform.position = Target.position + cameraRotation * CameraOffsetPosition;
        transform.rotation = cameraRotation;
    }
}