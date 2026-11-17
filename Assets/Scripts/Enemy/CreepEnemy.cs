using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CreepEnemy : EnemyBase, IDamageable
{
    public int MeleeDamage = 1;
    public float AttackCooldown = 1f;
    public float RotateSpeed = 180f;
    public UnityEvent OnMeleeAttack;
    private float _nextAttackTime;
    private Animator _anim;

    public override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
        AttackRange = Mathf.Max(AttackRange, 0f);
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

    public override void Fire()
    {

    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy is taken damage");
        _anim.SetTrigger("Hit");
    }
}
