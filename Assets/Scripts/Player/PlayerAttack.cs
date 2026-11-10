using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(Animator))]
public class PlayerAttack : MonoBehaviour
{
    private Animator _anim;
    private PlayerInput _playerInput;
    private InputSystem_Actions _inputAction;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (_inputAction != null)
        {
            _inputAction.Player.Attack.performed += Attack;
        }
    }

    private void Start()
    {
        _inputAction = _playerInput.InputActions;
    }

    private void OnDisable()
    {
        if (_inputAction != null)
        {
            _inputAction.Player.Attack.performed -= CastSkill1;
        }
    }

    private void Attack(InputAction.CallbackContext callbackContext)
    {
        _anim.SetBool("IsCombat", true);
        _anim.SetTrigger("MeleeAttack");
    }

    private void CastSkill1(InputAction.CallbackContext callbackContext)
    {
        if (_inputAction.Player.RightClick.IsPressed())
        {
            _anim.SetBool("IsCombat", true);
            _anim.SetTrigger("Skill1");
        }
    }

    public void IsCombatFisished()
    {
        _anim.SetBool("IsCombat", false);
    }
}