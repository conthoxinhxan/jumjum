using System;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; private set; } = 100;
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    public event Action<float> HealthChanged;
    public event Action Died;

    public void Initialize()
    {
        CurrentHealth = MaxHealth;
        HealthChanged.Invoke(CurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || !(damage > 0f))
            return;

        CurrentHealth = Mathf.Max(0f, CurrentHealth - damage);
        HealthChanged?.Invoke(CurrentHealth);

        if (IsDead) 
        {
            Died?.Invoke();
        }
            
    }
}