using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    private Animator _anim;
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private GameManager _gameManager;

    private Vector3 _moveDirection;
    private bool _isGrounded;
    private bool _isJumpRequested;

    private int _maxHealth = 3;
    private Vector2 _moveInput;

    private static readonly int IsRunHash = Animator.StringToHash("IsRun");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _gameManager = GameManager.Instance;
        _gameManager.OnGameStart += OnGameStart;
    }

    private void OnDisable()
    {
        if (_gameManager == null)
        {
            return;
        }

        _gameManager.OnGameStart -= OnGameStart;
        _gameManager = null;
    }

    private void Update()
    {
        HandleInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        CheckGround();
        // HandleMovement();
        HandleJump();
    }

    private void HandleInput()
    {
        _moveInput = _playerInput.MoveInput;
        // Vector3 inputDirection = new Vector3(input.x, 0f, input.y).normalized;

        // _moveDirection = transform.TransformDirection(inputDirection);

        if (_playerInput.JumpPressedThisFrame)
        {
            _isJumpRequested = true;
        }
    }

    /*
    private void HandleInputLegacy()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        _moveDirection = transform.TransformDirection(inputDirection);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isJumpRequested = true;
        }
    }
    */

    private void HandleMovement()
    {
        Vector3 velocity = _rb.linearVelocity;

        velocity.x = _moveDirection.x * _moveSpeed;
        velocity.z = _moveDirection.z * _moveSpeed;

        _rb.linearVelocity = velocity;
    }

    private void HandleJump()
    {
        if (!_isJumpRequested)
            return;

        if (_isGrounded)
        {
            Vector3 velocity = _rb.linearVelocity;
            velocity.y = _jumpForce;

            _rb.linearVelocity = velocity;
        }

        _isJumpRequested = false;
    }

    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundLayer);
    }

    private void UpdateAnimation()
    {
        _anim.SetFloat("MoveX", _moveInput.x, 0.1f, Time.deltaTime);
        _anim.SetFloat("MoveY", _moveInput.y, 0.1f, Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
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