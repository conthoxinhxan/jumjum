using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    private PlayerInput _input;
    private PlayerHealth _health;
    public event Action<int> HealingItemCountChanged;
    public event Action InventoryChanged;

    public List<UseableItem> UseableItems { get ; private set; } = new();
    public List<ItemBase> Inventory { get ; private set; } = new();

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

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.InteractPressed -= UseHealingItem;
        }
    }

    public void Initialize(PlayerInventoryData inventory)
    {
        Inventory.Clear();
        UseableItems.Clear();

        if (inventory?.Items == null || inventory.Items.Count == 0)
        {
            AddHealingItem(3);
            return;
        }

        foreach (var savedItem in inventory.Items)
        {
            if (savedItem == null || savedItem.Quantity < 0)
            {
                continue;
            }

            if (string.Equals(savedItem.Name, "Health Potion", StringComparison.OrdinalIgnoreCase))
            {
                AddHealingItem(savedItem.Quantity);
            }
            else
            {
                AddItem(new ItemBase
                {
                    Name = savedItem.Name,
                    Quantity = savedItem.Quantity,
                    Description = savedItem.Description
                });
            }
        }

        InventoryChanged?.Invoke();
    }

    public void AddItem(ItemBase newItem)
    {
        if (newItem == null || newItem.Quantity <= 0)
        {
            return;
        }

        foreach (var item in Inventory)
        {
            if (string.Equals(item.Name, newItem.Name, StringComparison.OrdinalIgnoreCase))
            {
                item.Quantity += newItem.Quantity;
                UpdateUseableItemQuantity(newItem.Name, item.Quantity);
                InventoryChanged?.Invoke();
                return;
            }
        }

        Inventory.Add(newItem);

        if (newItem is UseableItem useableItem)
        {
            UseableItems.Add(useableItem);
            SetCurrentUseable(useableItem);
        }

        InventoryChanged?.Invoke();
    }

    public void AddHealingItem(int quantity = 1)
    {
        if (quantity < 0)
        {
            return;
        }

        var healItem = new HealItem(_health, quantity);
        foreach (var item in Inventory)
        {
            if (string.Equals(item.Name, healItem.Name, StringComparison.OrdinalIgnoreCase))
            {
                item.Quantity += quantity;
                UpdateUseableItemQuantity(healItem.Name, item.Quantity);
                InventoryChanged?.Invoke();
                return;
            }
        }

        Inventory.Add(new ItemBase
        {
            Name = healItem.Name,
            Quantity = quantity,
            Description = healItem.Description
        });

        UseableItems.Add(healItem);
        SetCurrentUseable(healItem);
        InventoryChanged?.Invoke();
        HealingItemCountChanged?.Invoke(quantity);
    }

    private void UpdateUseableItemQuantity(string itemName, int quantity)
    {
        foreach (var useableItem in UseableItems)
        {
            if (string.Equals(useableItem.Name, itemName, StringComparison.OrdinalIgnoreCase))
            {
                useableItem.Quantity = quantity;
                return;
            }
        }

        if (string.Equals(itemName, "Health Potion", StringComparison.OrdinalIgnoreCase))
        {
            var healItem = new HealItem(_health, quantity);
            UseableItems.Add(healItem);
            SetCurrentUseable(healItem);
        }
    }

    private void UseHealingItem()
    {
        var currentItem = UseableItems.Find(item => item.IsCurrentUse);
        if (currentItem == null && UseableItems.Count > 0)
        {
            currentItem = UseableItems[0];
        }

        if (currentItem == null || currentItem.Quantity <= 0)
        {
            return;
        }

        currentItem.Use();
        currentItem.Quantity--;

        foreach (var item in Inventory)
        {
            if (string.Equals(item.Name, currentItem.Name, StringComparison.OrdinalIgnoreCase))
            {
                item.Quantity = currentItem.Quantity;
                break;
            }
        }

        HealingItemCountChanged?.Invoke(currentItem.Quantity);
        InventoryChanged?.Invoke();
    }

    private void SetCurrentUseable(UseableItem selectedItem)
    {
        foreach (var item in UseableItems)
        {
            item.IsCurrentUse = item == selectedItem;
        }
    }
}
