using System.Collections;
using UnityEngine;

public class CreepEnemy : EnemyBase
{
    public GameObject BulletPrefab;
    public float ShootRate = 0.8f;
    public float RotateSpeed = 180;

    private Rigidbody _rb;
    private GameObject _target;
    private Coroutine _shotingCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ScanAround();
    }

    void FixedUpdate()
    {
        if (_target == null)
        {
            return;
        }

        MoveTowardTarget();
        RotateTo(_target.transform.position);
    }

    private void ScanAround()
    {
        if (_target != null)
        {
            float distance = Vector3.Distance(transform.position, _target.transform.position);

            if (distance > ScanRange)
            {
                _target = null;

                if (_shotingCoroutine != null)
                {
                    StopCoroutine(_shotingCoroutine);
                    _shotingCoroutine = null;
                }
            }

            return;
        }

        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, ScanRange, ScanLayer);

        foreach (var obj in detectedObjects)
        {
            if (obj.CompareTag("Player"))
            {
                _target = obj.gameObject;

                if (_shotingCoroutine == null)
                {
                    _shotingCoroutine = StartCoroutine(ShootAtInterval());
                }

                break;
            }
        }
    }

    public void MoveTowardTarget()
    {
        Vector3 direction = _target.transform.position - _rb.position;
        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance <= AttackRange)
        {
            return;
        }

        float step = Mathf.Min(MoveSpeed * Time.fixedDeltaTime, distance - AttackRange);

        Vector3 newPosition = _rb.position + direction.normalized * step;

        _rb.MovePosition(newPosition);
    }

    public void RotateTo(Vector3 target)
    {
        Vector3 direction = target - ShootPoint.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, RotateSpeed * Time.fixedDeltaTime));
    }

    public override void Fire()
    {
        GameObject bulletObject = Instantiate(BulletPrefab, ShootPoint.position, ShootPoint.rotation);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Speed = 4;
        bullet.Fire(ShootPoint.forward);
    }

    public IEnumerator ShootAtInterval()
    {
        while (true)
        {
            if (_target != null)
            {
                if (Physics.Raycast(ShootPoint.position, ShootPoint.forward, out RaycastHit hit, AttackRange, ScanLayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Fire();
                    }
                }
            }

            yield return new WaitForSeconds(ShootRate);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ScanRange);
    }
}