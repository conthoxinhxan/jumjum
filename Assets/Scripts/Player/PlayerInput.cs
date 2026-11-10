using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    public InputSystem_Actions InputActions => _inputActions;

    public Vector2 MoveInput => _inputActions.Player.Move.ReadValue<Vector2>();
    public Vector2 LookInput => _inputActions.Player.Look.ReadValue<Vector2>();
    public bool JumpPressedThisFrame => _inputActions.Player.Jump.WasPressedThisFrame();

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }
}