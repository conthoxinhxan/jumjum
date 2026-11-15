using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CreepEnemy : EnemyBase
{
    public int MeleeDamage = 1;
    public float AttackCooldown = 1f;
    public float RotateSpeed = 180f;
    public UnityEvent OnMeleeAttack;
    private float _nextAttackTime;

    public override void Awake()
    {
        base.Awake();
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
        // if (Target == null || !IsTargetInAttackRange())
        //     return;

        // MonoBehaviour[] components = Target.GetComponents<MonoBehaviour>();
        // bool damagedTarget = false;
        // foreach (MonoBehaviour component in components)
        // {
        //     if (component is IDamageable damageable)
        //     {
        //         damageable.TakeDamage(MeleeDamage);
        //         damagedTarget = true;
        //         break;
        //     }
        // }

        // if (!damagedTarget)
        // {
        //     Debug.LogWarning($"Target '{Target.name}' has no IDamageable component.", Target);
        // }

        // OnMeleeAttack?.Invoke();
    }
}
