using UnityEngine;

public abstract class EnemyBase : MonoBehaviour 
{
    public float MoveSpeed = 3f;
    public float AttackRange = 2f;
    public float ScanRange = 3f;
    public Transform ShootPoint;
    public LayerMask ScanLayer;

    public abstract void Fire();
}