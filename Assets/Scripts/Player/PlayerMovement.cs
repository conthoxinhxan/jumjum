using System;
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
    private bool _isGrounded;
    private bool _isJumpRequested;

    private Vector2 _moveInput;

    public event Action Jumped;
    public event Action<bool> GroundedChanged;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerInput.JumpPressed += RequestJump;
    }

    private void OnDisable()
    {
        _playerInput.JumpPressed -= RequestJump;
        _isJumpRequested = false;
    }

    private void Update()
    {
        _moveInput = _playerInput.MoveInput;
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        CheckGround();
        HandleJump();
    }

    private void RequestJump() => _isJumpRequested = true;

    private void HandleJump()
    {
        if (!_isJumpRequested || !_playerInput.isActiveAndEnabled)
        {
            return;
        }


        if (_isGrounded)
        {
            Vector3 velocity = _rb.linearVelocity;
            velocity.y = _jumpForce;

            _rb.linearVelocity = velocity;
            Jumped?.Invoke();
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
            GameManager.Instance?.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

}