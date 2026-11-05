using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreepEnemy : MonoBehaviour
{
    public float MoveSpeed = 3f;
    public float AttackRange = 2f;
    public float ScanRange = 3f;
    public Transform ShotPoint;
    public LayerMask ScanLayer;

    private Rigidbody _rb;
    private GameObject _target;

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
    }

    private void ScanAround()
    {
        if (_target != null)
        {
            return;
        }
        Collider[] detectedObject = Physics.OverlapSphere(transform.position, ScanRange, ScanLayer);

        if (detectedObject.Count() > 0)
        {
            foreach (var obj in detectedObject)
            {
                if (obj.CompareTag("Player"))
                {
                    _target = obj.gameObject;
                }
            }
        }
        else
        {
            _target = null;
        }
    }

    public void MoveTowardTarget()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) <= AttackRange)
        {
            return;
        }
        Vector3 newPosition = Vector3.MoveTowards(_rb.position, _target.transform.position, MoveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(newPosition);
    }

    // public IEnumerator RotateTo(Vector3 targetRotation)
    // {
        
    // }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ScanRange);
    }
}