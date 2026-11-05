using UnityEngine;

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

    private void Start()
    {
        if (Target == null)
        {
            return;
        }

        // Khởi tạo giá trị _mouseX bằng góc quay hiện tại của Target quanh trục Y.
        _mouseX = Target.eulerAngles.y;

        // Cursor.lockState = CursorLockMode.Locked; khóa con trỏ chuột vào trung tâm màn hình, 
        // ngăn người dùng di chuyển nó ra khỏi cửa sổ trò chơi.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Cập nhật giá trị _mouseX và _mouseY dựa trên đầu vào chuột, 
        // với độ nhạy được xác định bởi MouseSensitivity.
        _mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        _mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        // Giới hạn giá trị _mouseY để tránh camera quay quá cao hoặc quá thấp.
        _mouseY = Mathf.Clamp(_mouseY, MinPitch, MaxPitch);

        // Player chỉ quay ngang quanh trục Y.
        Target.rotation = Quaternion.Euler(0f, _mouseX, 0f);
    }

    private void LateUpdate()
    {
        Quaternion cameraRotation = Quaternion.Euler(_mouseY, _mouseX, 0f);

        // Xoay offset theo hướng nhìn rồi cộng vào vị trí Target.
        transform.position = Target.position + cameraRotation * CameraOffsetPosition;
        transform.rotation = cameraRotation;
    }
}