using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AutoShotingRobot : MonoBehaviour
{
    public GameObject Head;
    public Transform ShootPoint;
    public float RotateSpeed = 90f; // 90 degrees per second
    public float ShootInterval = 1f; // Shoot every 1 second
    public float DetectionRange = 10f; // Range to detect the player

    public Bullet BulletPrefab;

    public LayerMask PlayerLayer;

    private IEnumerator _shootingCoroutine;
    private IEnumerator _rotationCoroutine;


    void Update()
    {
        CheckPlayerInSight();
    }

    public void CheckPlayerInSight()
    {
        Collider[] hits = Physics.OverlapSphere(ShootPoint.position, 10f, PlayerLayer);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    if (_shootingCoroutine == null)
                    {
                        _shootingCoroutine = ShootAtInterval();
                        StartCoroutine(_shootingCoroutine);
                    }

                    if (_rotationCoroutine == null)
                    {
                        _rotationCoroutine = RotateHeadTowardTarget(hit.transform);
                        StartCoroutine(_rotationCoroutine);
                    }
                    break;
                }
            }
        }
        else 
        {
            StopAllCoroutines();
            _shootingCoroutine = null;
            _rotationCoroutine = null;
        }
    }

    public void Shoot()
    {
        Bullet bullet = Instantiate(BulletPrefab, ShootPoint.position, ShootPoint.rotation);
        bullet.Fire(ShootPoint.forward);
    }

    public IEnumerator ShootAtInterval()
    {
        while (true)
        {
            RaycastHit[] hits = Physics.RaycastAll(ShootPoint.position, ShootPoint.forward, DetectionRange, PlayerLayer);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Shoot();
                    break;
                }
            }
            yield return new WaitForSeconds(ShootInterval);
        }
    }

    public IEnumerator RotateHeadTowardTarget(Transform target)
    {
        while (true)
        {
            Vector3 direction = (target.position - Head.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Head.transform.rotation = Quaternion.RotateTowards(Head.transform.rotation, lookRotation, RotateSpeed * Time.deltaTime);

            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ShootPoint.position, DetectionRange);
    }
}