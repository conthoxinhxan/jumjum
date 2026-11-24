using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CreepEnemy : EnemyBase, IDamageable
{
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");

    public int MeleeDamage = 1;
    public float AttackCooldown = 1f;
    public float RotateSpeed = 180f;
    public float MaxHealth = 80f;
    public UnityEvent OnMeleeAttack;
    public Slider HealthSlider;
    private float _nextAttackTime;
    private Animator _anim;
    private float _currentHealth;

    public override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
        AttackRange = Mathf.Max(AttackRange, 0f);
        HealthSlider.value = 1;
        _currentHealth = MaxHealth;
    }

    protected virtual void Update()
    {
        UpdateTarget();

        if (Target != null && IsTargetInAttackRange() && Time.time >= _nextAttackTime)
        {
            Fire();
            _nextAttackTime = Time.time + AttackCooldown;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (Target == null)
            return;

        MoveTowardTarget();
        RotateTowardTarget(RotateSpeed);
    }

    private bool IsTargetInAttackRange()
    {
        Vector3 offset = Target.position - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= AttackRange * AttackRange;
    }

    public void Fire()
    {
        if (Target == null || !IsTargetInAttackRange())
        {
            return;
        }
            
        _anim.SetTrigger(AttackTriggerHash);
        OnMeleeAttack?.Invoke();

        IDamageable target = Target.GetComponent<IDamageable>();
        target.TakeDamage(MeleeDamage);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy is taken damage");
        _anim.SetTrigger("Hit");
        _currentHealth -= damage;
        HealthSlider.value = _currentHealth / MaxHealth;
        if (_currentHealth <= 0)
        {
            Debug.Log("this enemy is dead");
            // TODO : animatin Dead for 2 sec then disapear
        }
    }
}
