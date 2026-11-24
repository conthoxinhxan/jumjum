using TMPro;
using UnityEngine;

public class PlayerItemUsePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    private PlayerItemUse _itemUse;

    public void Initialize(PlayerItemUse itemUse)
    {
        if (_itemUse != null)
        {
            _itemUse.HealingItemCountChanged -= UpdateCount;
        }

        _itemUse = itemUse;
        _itemUse.HealingItemCountChanged += UpdateCount;
        UpdateCount(_itemUse.HealingItemCount);
    }

    private void OnDestroy()
    {
        if (_itemUse != null)
        {
            _itemUse.HealingItemCountChanged -= UpdateCount;
        }
    }

    private void UpdateCount(int count)
    {
        _label.text = $"[E] Health Potion x{count}";
    }
}
