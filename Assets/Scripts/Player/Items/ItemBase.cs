using System;

public interface IUseableItem
{
    void Use();
}

[Serializable]
public class ItemBase
{
    public string Name;
    public int Quantity;
    public string Description;
    public string AddressableKeyUI;
    public string AddressableKeyGameObject;
    public string Type;
}

[Serializable]
public abstract class UseableItem : ItemBase
{
    public bool IsCurrentUse;
    public abstract void Use();
}

[Serializable]
public class HealItem : UseableItem
{
    private PlayerHealth _playerHealth;
    private float percentage = 0.25f;

    public HealItem()
    {
    }

    public HealItem(PlayerHealth playerHealth, int quantity = 1)
    {
        _playerHealth = playerHealth;
        Name = "Health Potion";
        Quantity = quantity;
        Description = "Restores 25% of maximum health.";
    }

    public override void Use()
    {
        if (_playerHealth != null)
        {
            _playerHealth.Heal(percentage);
        }
    }
}