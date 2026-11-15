using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class MinibossEnemy : EnemyBase
{
    [Header("Movement")]
    public float RotateSpeed = 120f;

    [Header("Skill 1 - Heavy strike")]
    public int SkillOneDamage = 2;
    public float SkillOneRange = 2.5f;
    public float SkillOneCooldown = 3f;
    public UnityEvent OnSkillOne;

    [Header("Skill 2 - Ground slam")]
    public int SkillTwoDamage = 3;
    public float SkillTwoRadius = 4f;
    public float SkillTwoCooldown = 8f;
    public UnityEvent OnSkillTwo;

    private Animator _anim;
    private float _nextSkillOneTime;
    private float _nextSkillTwoTime;

    public override void Awake()
    {
        base.Awake();
        _anim = GetComponent<Animator>();
        AttackRange = Mathf.Max(AttackRange, 0f);
    }

    private void Update()
    {
        UpdateTarget();

        if (Target == null)
            return;

        // float distance = GetTargetDistance();
        // if (distance <= SkillOneRange && Time.time >= _nextSkillOneTime)
        // {
        //     UseSkillOne();
        // }
        // else if (distance <= SkillTwoRadius && Time.time >= _nextSkillTwoTime)
        // {
        //     UseSkillTwo();
        // }
    }

    private void FixedUpdate()
    {
        if (Target == null)
            return;

        MoveTowardTarget();
        RotateTowardTarget(RotateSpeed);
    }

    public override void Fire()
    {
        // UseSkillOne();
    }

    // public void UseSkillOne()
    // {
    //     if (Target == null || GetTargetDistance() > SkillOneRange)
    //         return;

    //     ApplyDamage(Target, SkillOneDamage);
    //     OnSkillOne?.Invoke();
    //     _nextSkillOneTime = Time.time + SkillOneCooldown;
    // }

    // public void UseSkillTwo()
    // {
    //     if (Target == null || GetTargetDistance() > SkillTwoRadius)
    //         return;

    //     Collider[] colliders = Physics.OverlapSphere(transform.position, SkillTwoRadius, ScanLayer);
    //     HashSet<Transform> damagedPlayers = new HashSet<Transform>();

    //     foreach (Collider detectedCollider in colliders)
    //     {
    //         Transform player = FindPlayerRoot(detectedCollider.transform);
    //         if (player == null || !damagedPlayers.Add(player))
    //             continue;

    //         ApplyDamage(player, SkillTwoDamage);
    //     }

    //     OnSkillTwo?.Invoke();
    //     _nextSkillTwoTime = Time.time + SkillTwoCooldown;
    // }

    private float GetTargetDistance()
    {
        Vector3 offset = Target.position - transform.position;
        offset.y = 0f;
        return offset.magnitude;
    }

    // private static void ApplyDamage(Transform target, int damage)
    // {
    //     MonoBehaviour[] components = target.GetComponents<MonoBehaviour>();
    //     foreach (MonoBehaviour component in components)
    //     {
    //         if (component is IDamageable damageable)
    //         {
    //             damageable.TakeDamage(damage);
    //             return;
    //         }
    //     }

    //     Debug.LogWarning($"Target '{target.name}' has no IDamageable component.", target);
    // }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SkillOneRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, SkillTwoRadius);
    }
}
