using UnityEngine;

public class AutoShotingRobot : EnemyBase
{
    public Transform Head;
    public float RotateSpeed = 90f;
    public float ShootInterval = 1f;
    public Bullet BulletPrefab;

    private float _nextShotTime;

    private void Update()
    {
        UpdateTarget();
        if (Target == null)
            return;

        RotateHeadTowardTarget();

        if (Time.time >= _nextShotTime && HasLineOfSight())
        {
            Fire();
            _nextShotTime = Time.time + ShootInterval;
        }
    }

    private void RotateHeadTowardTarget()
    {
        Transform head = Head != null ? Head : transform;
        Vector3 direction = Target.position - head.position;
        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        head.rotation = Quaternion.RotateTowards(
            head.rotation,
            targetRotation,
            RotateSpeed * Time.deltaTime);
    }

    private bool HasLineOfSight()
    {
        if (ShootPoint == null)
            return false;

        Vector3 direction = Target.position - ShootPoint.position;
        float distance = direction.magnitude;
        if (distance > AttackRange || distance <= Mathf.Epsilon)
            return false;

        if (!Physics.Raycast(
                ShootPoint.position,
                direction / distance,
                out RaycastHit hit,
                distance,
                ScanLayer))
        {
            return false;
        }

        return FindPlayerRoot(hit.transform) == Target;
    }

    public void Fire()
    {
        if (BulletPrefab == null || ShootPoint == null)
        {
            Debug.LogError("AutoShotingRobot needs a BulletPrefab and ShootPoint.", this);
            return;
        }

        Bullet bullet = Instantiate(BulletPrefab, ShootPoint.position, ShootPoint.rotation);
        bullet.Fire(ShootPoint.forward);
    }
}
