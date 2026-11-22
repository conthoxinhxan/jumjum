using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    public event Action AttackPressed;
    public event Action JumpPressed;
    public event Action InteractPressed;

    public InputSystem_Actions InputActions => _inputActions;

    public Vector2 MoveInput => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookInput => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool JumpPressedThisFrame => _inputActions.Player.Jump.WasPressedThisFrame();

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.Attack.performed += HandleAttack;
        _inputActions.Player.Jump.performed += HandleJump;
        _inputActions.Player.Interact.performed += HandleInteract;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void OnDestroy()
    {
        _inputActions.Player.Attack.performed -= HandleAttack;
        _inputActions.Player.Jump.performed -= HandleJump;
        _inputActions.Player.Interact.performed -= HandleInteract;
        _inputActions.Dispose();
    }

    private void HandleAttack(InputAction.CallbackContext context) => AttackPressed?.Invoke();
    private void HandleJump(InputAction.CallbackContext context) => JumpPressed?.Invoke();
    private void HandleInteract(InputAction.CallbackContext context) => InteractPressed?.Invoke();
}