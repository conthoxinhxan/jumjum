using System;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0f;

    public event Action<float> HealthChanged;
    public event Action Died;

    public void Initialize(float currentHealth, float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        HealthChanged?.Invoke(CurrentHealth);
    }

    // public bool TryHeal(float amount)
    // {
    //     if (IsDead || CurrentHealth >= MaxHealth || !(amount > 0f))
    //     {
    //         return false;
    //     }
    //     float healedHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
    //     if (healedHealth <= CurrentHealth)
    //     {
    //         return false;
    //     }

    //     CurrentHealth = healedHealth;
    //     HealthChanged?.Invoke(CurrentHealth);
    //     return true;
    // }

    public void Heal(float percentage) // 0 -> 1
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + MaxHealth * percentage);
        HealthChanged?.Invoke(CurrentHealth);
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
