using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float RotateSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime);
    }
}