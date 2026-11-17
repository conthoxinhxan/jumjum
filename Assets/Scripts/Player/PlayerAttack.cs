
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Transform _attackPoint;

    [SerializeField]
    private float _attackRadius;

    [SerializeField]
    private float _attackDamage;

    [SerializeField]
    private LayerMask _enemyLayer;

    private Animator _anim;
    private InputSystem_Actions _inputAction;
    private readonly int[] _attackStateHashes =
    {
        Animator.StringToHash("Attack1"),
        Animator.StringToHash("Attack2"),
        Animator.StringToHash("Attack3")
    };

    private int _comboIndex = 0;
    private bool _canCombo = false;
    private bool _queuedAttack = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        PlayerInput playerInput = GetComponent<PlayerInput>();
        _inputAction = playerInput.InputActions;
        _inputAction.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        if (_inputAction != null)
        {
            _inputAction.Player.Attack.performed -= Attack;
        }
    }

    private void Update()
    {
        if (_comboIndex == 0 || _anim.IsInTransition(0))
        {
            return;
        }

        AnimatorStateInfo state = _anim.GetCurrentAnimatorStateInfo(0);
        int attackIndex = _comboIndex - 1;
        if (attackIndex < _attackStateHashes.Length
            && state.shortNameHash == _attackStateHashes[attackIndex]
            && state.normalizedTime >= 1f)
        {
            if (_queuedAttack && _comboIndex < 3)
            {
                _canCombo = true;
                NextAttack();
            }
            else
            {
                EndAttack(_comboIndex);
            }
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        // Chưa đánh: bắt đầu combo
        if (_comboIndex == 0)
        {
            _comboIndex = 1;
            _queuedAttack = false;
            _canCombo = false;

            _anim.SetBool("IsCombat", true);
            _anim.SetTrigger("Attack1");
            return;
        }

        // Đang thực hiện đòn 1 hoặc 2
        if (_comboIndex < 3)
        {
            _queuedAttack = true;

            if (_canCombo)
            {
                NextAttack();
            }
        }
    }

    private void NextAttack()
    {
        if (!_queuedAttack || !_canCombo || _comboIndex >= 3)
        {
            return;
        }

        _comboIndex++;

        _queuedAttack = false;
        _canCombo = false;

        _anim.SetTrigger("Attack" + _comboIndex);
    }

    public void EndAttack(int attackIndex)
    {
        if (_comboIndex != attackIndex)
            return;

        _comboIndex = 0;
        _queuedAttack = false;
        _canCombo = false;

        _anim.SetBool("IsCombat", false);
    }

    public void DealDamage()
    {
        Debug.Log("Deal damage");
        var hits = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _enemyLayer);

        var damagedTargets = new HashSet<IDamageable>();

        foreach (var hit in hits)
        {
             IDamageable target = hit.GetComponentInParent<IDamageable>();

            if (target != null && damagedTargets.Add(target))
                target.TakeDamage(_attackDamage);
        }
    }
}
