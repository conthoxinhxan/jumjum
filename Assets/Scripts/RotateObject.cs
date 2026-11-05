using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float RotateSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Quay đối tượng quanh trục Y với tốc độ RotateSpeed, nhân với Time.deltaTime để đảm bảo tốc độ quay
        //  không phụ thuộc vào tốc độ khung hình.
        transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
    }
}