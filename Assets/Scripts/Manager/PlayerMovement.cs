using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float JumpForce = 5f;
    public float MoveSpeed = 5f;

    [SerializeField] private Transform _groundCheck;
    [SerializeField]private LayerMask _groundLayer;

    private Rigidbody _rb;
    private Animator _anim;
    private Vector3 _moveDirection;
    private bool _isJumpRequested;
    private int _maxHealth = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        GameManager.Instance.OnGameStart += OnGameStart;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var direction = new Vector3(horizontal, 0, vertical).normalized;
        _moveDirection = transform.TransformDirection(direction);

        // Kiểm tra nếu người chơi nhấn phím Space và đang đứng trên mặt đất, thì đặt _isJumpRequested thành true để yêu cầu nhảy.
        if (Input.GetKeyDown(KeyCode.Space) && IsGround())
        {
            _isJumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Cập nhật vận tốc của Rigidbody dựa trên hướng di chuyển và tốc độ di chuyển.
        _rb.linearVelocity = new Vector3(
            _moveDirection.x * MoveSpeed,
            _rb.linearVelocity.y,
            _moveDirection.z * MoveSpeed
        );

        // Kiểm tra nếu người chơi đã yêu cầu nhảy và đang đứng trên mặt đất.
        if (_isJumpRequested && IsGround())
        {
            _rb.linearVelocity = new Vector3(
                _rb.linearVelocity.x,
                JumpForce,
                _rb.linearVelocity.z
            );
            _isJumpRequested = false;
        }
    }

    private bool IsGround()
    {
        return Physics.CheckSphere(_groundCheck.position, .1f, _groundLayer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("DeathZone"))
        {
            GameManager.Instance.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance.GetCoin();
            Destroy(other.gameObject);
        }
    }

    public void OnGameStart(int maxHealth)
    {
        _maxHealth = maxHealth;
    }
}
