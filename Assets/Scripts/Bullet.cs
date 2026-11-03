using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 8f;
    
    private Rigidbody _rb;

    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        if (_direction != Vector3.zero)
        {
            transform.position += _direction * Speed * Time.deltaTime;
        }
    }

    public void Fire(Vector3 direction)
    {
        _direction = direction;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
