using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Weapon,
    Heal,
    Mana,
    Buff,
    Other
}

public class ItemPanel : MonoBehaviour
{
    public Image Icon;
    public TMP_Text QuantityText;
    public ItemType ItemType;

    public void Initialize(ItemBase item)
    {
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        ItemType = ItemTypeForName(item.Name);

        if (QuantityText != null)
        {
            QuantityText.text = $"{item.Quantity}";
        }
    }

    private static ItemType ItemTypeForName(string itemName)
    {
        if (string.Equals(itemName, "Health Potion", System.StringComparison.OrdinalIgnoreCase))
        {
            return ItemType.Heal;
        }

        return ItemType.Other;
    }
}