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

    void Start()
    {
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
        UpdateCameraRotation();
    }

    private void UpdatePlayerRotation()
    {
        Player.rotation = Quaternion.Euler(
            0,
            _mouseX,
            0
        );
    }

    private void UpdateCameraRotation()
    {
        CameraTarget.localRotation = Quaternion.Euler(
            _mouseY,
            0,
            0
        );
    }
}