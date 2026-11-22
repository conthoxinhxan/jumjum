
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(Animator))]
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
    private PlayerInput _playerInput;

    public event Action<int> AttackStarted;
    public event Action<int> AttackEnded;
    public event Action<IDamageable> TargetHit;
    private readonly int[] _attackStateHashes =
    {
        Animator.StringToHash("Attack1"),
        Animator.StringToHash("Attack2"),
        Animator.StringToHash("Attack3")
    };

    private int _comboIndex = 0;
    private bool _canCombo = false;
    private bool _queuedAttack = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        _playerInput.AttackPressed += Attack;
    }

    private void OnDisable()
    {
        _playerInput.AttackPressed -= Attack;
        if (_comboIndex != 0)
        {
            foreach (int attackHash in _attackStateHashes)
                _anim.ResetTrigger(attackHash);

            EndAttack(_comboIndex);
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

    private void Attack()
    {
        // Chưa đánh: bắt đầu combo
        if (_comboIndex == 0)
        {
            _comboIndex = 1;
            _queuedAttack = false;
            _canCombo = false;

            _anim.SetBool("IsCombat", true);
            _anim.SetTrigger(_attackStateHashes[0]);
            AttackStarted?.Invoke(_comboIndex);
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

        _anim.SetTrigger(_attackStateHashes[_comboIndex - 1]);
        AttackStarted?.Invoke(_comboIndex);
    }

    public void EndAttack(int attackIndex)
    {
        if (_comboIndex != attackIndex)
            return;

        _comboIndex = 0;
        _queuedAttack = false;
        _canCombo = false;

        _anim.SetBool("IsCombat", false);
        AttackEnded?.Invoke(attackIndex);
    }

    public void DealDamage()
    {
        if (!isActiveAndEnabled || !_playerInput.isActiveAndEnabled || _attackPoint == null)
            return;

        var hits = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _enemyLayer);

        var damagedTargets = new HashSet<IDamageable>();

        foreach (var hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();

            if (target != null && damagedTargets.Add(target))
            {
                target.TakeDamage(_attackDamage);
                TargetHit?.Invoke(target);
            }
        }
    }
}
