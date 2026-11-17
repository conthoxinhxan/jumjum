using System.Runtime.CompilerServices;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
}

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Common stats")]
    public float MoveSpeed = 3f;
    public float AttackRange = 2f;
    public float ScanRange = 8f;
    public Transform ShootPoint;
    public LayerMask ScanLayer;

    private Rigidbody _rb;
    private bool _isAttack = false;

    protected Transform Target { get; private set; }

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();    
    }

    protected void UpdateTarget()
    {
        if (Target != null)
        {
            Vector3 offset = Target.position - transform.position;
            offset.y = 0f;
            if (offset.sqrMagnitude <= ScanRange * ScanRange)
                return;

            Target = null;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, ScanRange, ScanLayer);
        float nearestDistance = float.PositiveInfinity;

        foreach (Collider detectedCollider in colliders)
        {
            Transform player = FindPlayerRoot(detectedCollider.transform);
            if (player == null)
                continue;

            float distance = (player.position - transform.position).sqrMagnitude;
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                Target = player;
            }
        }
    }

    protected bool MoveTowardTarget()
    {
        if (Target == null)
        {
            return false;
        }
        Vector3 direction = Target.position - transform.position;
        direction.y = 0f;
        float distance = direction.magnitude;

        if (distance <= AttackRange || distance <= Mathf.Epsilon)
            return false;

        float step = Mathf.Min(MoveSpeed * Time.fixedDeltaTime, distance - AttackRange);
        _rb.MovePosition(transform.position + direction / distance * step);
        return true;
    }

    protected void RotateTowardTarget(float rotateSpeed)
    {
        if (Target == null)
            return;

        Vector3 direction = Target.position - _rb.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _rb.MoveRotation(Quaternion.RotateTowards(
            _rb.rotation,
            targetRotation,
            rotateSpeed * Time.fixedDeltaTime));
    }

    protected static Transform FindPlayerRoot(Transform candidate)
    {
        while (candidate != null)
        {
            if (candidate.CompareTag("Player"))
                return candidate;

            candidate = candidate.parent;
        }

        return null;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ScanRange);
    }

    public abstract void Fire();
}
