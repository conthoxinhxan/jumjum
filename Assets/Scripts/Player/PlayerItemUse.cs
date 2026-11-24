using System;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField, Min(0)] private int _startingHealingItems = 3;
    [SerializeField, Min(1f)] private float _healthPerItem = 20f;

    private PlayerInput _input;
    private PlayerHealth _health;

    public int HealingItemCount { get; private set; }
    public event Action<int> HealingItemCountChanged;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _health = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (_input != null)
        {
            _input.InteractPressed += UseHealingItem;
        }
            
    }

    private void OnDisable() => _input.InteractPressed -= UseHealingItem;

    public void Initialize(PlayerInventoryData inventory)
    {
        HealingItemCount = Mathf.Max(0, inventory?.HealingItems ?? _startingHealingItems);
        HealingItemCountChanged?.Invoke(HealingItemCount);
    }

    public void AddHealingItem()
    {
        HealingItemCount++;
        HealingItemCountChanged?.Invoke(HealingItemCount);
    }

    private void UseHealingItem()
    {
        Debug.Log("Using healing item");
        if (HealingItemCount <= 0)
        {
            return;
        }

        HealingItemCount--;
        if (!_health.TryHeal(_healthPerItem))
        {
            HealingItemCount++;
            return;
        }

        HealingItemCountChanged?.Invoke(HealingItemCount);
    }
}
