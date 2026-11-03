using UnityEngine;

public enum CameraMode
{
    ThirdPerson,
    FirstPerson
}

public class CameraControl : MonoBehaviour
{
    public Transform Target;
    public CameraMode Mode = CameraMode.ThirdPerson;

    [Header("Third Person")]
    public float ThirdPersonDistance = 4f;
    public float ShoulderOffset = 0.6f;
    public float CameraHeight = 1.6f;

    [Header("First Person")]
    public float FirstPersonDistance = 1.7f;

    [Header("Mouse")]
    public float MouseSensitivity = 3f;
    
    // MinPitch và MaxPitch được sử dụng để giới hạn góc quay của camera theo trục X (ngang) khi người chơi di chuyển chuột lên hoặc xuống.
    public float MinPitch = -40f;
    public float MaxPitch = 60f;

    // _mouseX và _mouseY được sử dụng để lưu trữ giá trị góc quay của camera dựa trên chuyển động của chuột.
    private float _mouseX;
    private float _mouseY;

    void Start()
    {
        //Khởi tạo giá trị ban đầu cho _mouseX dựa trên góc quay hiện tại của đối tượng Target (người chơi).
        _mouseX = Target.eulerAngles.y;

        // Khóa con trỏ chuột vào trung tâm màn hình và ẩn con trỏ để tạo trải nghiệm chơi game tốt hơn.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Cập nhật giá trị _mouseX và _mouseY dựa trên chuyển động của chuột. 
        // Input.GetAxis("Mouse X") trả về giá trị chuyển động ngang của chuột,
        // và Input.GetAxis("Mouse Y") trả về giá trị chuyển động dọc của chuột. 
        // Giá trị này được nhân với MouseSensitivity để điều chỉnh độ nhạy của camera.
        _mouseX += Input.GetAxis("Mouse X") * MouseSensitivity;
        _mouseY -= Input.GetAxis("Mouse Y") * MouseSensitivity;

        Debug.Log($"Mouse X: {_mouseX}, Mouse Y: {_mouseY}");

        // Giới hạn giá trị _mouseY để đảm bảo rằng camera không quay quá cao hoặc quá thấp.
        _mouseY = Mathf.Clamp(_mouseY, MinPitch, MaxPitch);

        UpdatePlayerRotation();
    }

    void LateUpdate()
    {
        // Cập nhật vị trí và góc quay của camera dựa trên chế độ hiện tại (ThirdPerson hoặc FirstPerson).
        switch (Mode)
        {
            case CameraMode.ThirdPerson:
                UpdateThirdPersonCamera();
                break;

            case CameraMode.FirstPerson:
                UpdateFirstPersonCamera();
                break;
        }
    }

    private void UpdateThirdPersonCamera()
    {
        // Tạo một Quaternion đại diện cho góc quay của camera dựa trên giá trị _mouseX và _mouseY.
        Quaternion cameraRotation = Quaternion.Euler(_mouseY, _mouseX, 0);

        // Tính toán vị trí mong muốn của camera dựa trên vị trí của Target (người chơi), 
        // góc quay của camera, khoảng cách từ camera đến người chơi, và độ lệch vai.
        Vector3 pivot = Target.position + Vector3.up * CameraHeight;

        // Tính toán vị trí mong muốn của camera dựa trên pivot, góc quay của camera, 
        // khoảng cách từ camera đến người chơi, và độ lệch vai.
        Vector3 desiredPosition = pivot
            - cameraRotation * Vector3.forward * ThirdPersonDistance
            + cameraRotation * Vector3.right * ShoulderOffset;

        // Cập nhật vị trí và góc quay của camera dựa trên vị trí mong muốn và góc quay đã tính toán.
        transform.position = desiredPosition;
        transform.rotation = cameraRotation;
    }

    private void UpdateFirstPersonCamera()
    {
        // Tạo một Quaternion đại diện cho góc quay của camera dựa trên giá trị _mouseX và _mouseY.
        Quaternion cameraRotation = Quaternion.Euler(_mouseY, _mouseX, 0);
        // Cập nhật vị trí của camera dựa trên vị trí của Target (người chơi) và khoảng cách từ camera đến người chơi.
        transform.position = Target.position + Vector3.up * FirstPersonDistance;
        // Cập nhật góc quay của camera dựa trên góc quay đã tính toán.
        transform.rotation = cameraRotation;
    }

    // Cập nhật góc quay của đối tượng Target (người chơi) dựa trên giá trị _mouseX,
    // giúp người chơi quay theo hướng mà camera đang nhìn.
    private void UpdatePlayerRotation()
    {
        // Quaternion là một cấu trúc dữ liệu trong Unity đại diện cho một góc quay trong không gian 3D.
        // Ở đây, chúng ta tạo một Quaternion mới với góc quay chỉ thay đổi theo trục Y (quay ngang) dựa trên giá trị _mouseX,
        // giữ nguyên các góc quay theo trục X và Z.
        // Quaternion.Euler(Vector3) là một phương thức để tạo ra một Quaternion từ các góc quay Euler (theo độ).
        // Cuối cùng, chúng ta gán Quaternion này cho Target.rotation để cập nhật góc quay của đối tượng Target (người chơi).
        Target.rotation = Quaternion.Euler(0, _mouseX, 0);
    }
}