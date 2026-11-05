using Unity.Cinemachine;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Transform CameraTarget;

    [SerializeField] private float MouseSensitivity = 3f;
    [SerializeField] private float MinPitch = -40f;
    [SerializeField] private float MaxPitch = 60f;

    private float _mouseX;
    private float _mouseY;
    private CinemachineFollow _cameraFollow;
    private Vector3 _followOffset;

    void Start()
    {
        if (Player == null || CameraTarget == null)
        {
            Debug.LogError("TPSCameraController requires both Player and CameraTarget.", this);
            enabled = false;
            return;
        }

        _cameraFollow = CameraTarget.GetComponent<CinemachineFollow>();
        if (_cameraFollow == null)
        {
            Debug.LogError("CameraTarget must have a CinemachineFollow component.", CameraTarget);
            enabled = false;
            return;
        }

        _followOffset = _cameraFollow.FollowOffset;
        _mouseX = Player.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mouseX += mouseX * MouseSensitivity;
        _mouseY -= mouseY * MouseSensitivity;

        _mouseY = Mathf.Clamp(_mouseY, MinPitch, MaxPitch);
        UpdatePlayerRotation();
        UpdateCameraPosition();
    }

    private void UpdatePlayerRotation()
    {
        Player.rotation = Quaternion.Euler(
            0,
            _mouseX,
            0
        );
    }

    // update theo dõi vị trí của player
    private void UpdateCameraPosition()
    {
        _cameraFollow.FollowOffset = Quaternion.Euler(_mouseY, 0f, 0f) * _followOffset;
    }
}