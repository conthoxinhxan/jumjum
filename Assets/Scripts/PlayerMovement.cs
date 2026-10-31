using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float JumpForce = 5f;
    public float MoveSpeed = 5f;

    [SerializeField]
    private Transform _groundCheck;
    [SerializeField]
    private LayerMask _groundLayer;

    private Rigidbody _rb;
    private Vector3 _direction;
    private bool _isGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _direction = new Vector3(horizontal, 0, vertical).normalized;

        _rb.linearVelocity = new Vector3(_direction.x * MoveSpeed,
            _rb.linearVelocity.y,
            _direction.z * MoveSpeed
        );

        if (Input.GetKeyDown(KeyCode.Space) && IsGround())
        {
            Debug.Log("Jumping");

            _rb.linearVelocity = new Vector3(
                _rb.linearVelocity.x,
                JumpForce,
                _rb.linearVelocity.z
            );
        }
    }

    private bool IsGround()
    {
        return Physics.CheckSphere(_groundCheck.position, .1f, _groundLayer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }
}
