using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UseItemPanel : MonoBehaviour
{
    public Transform Container;

    private List<ItemPanel> _useItemPanels = new();
    private List<UseableItem> _useItems = new();
    private PlayerItemUse _playerItemUse;
    private bool _isRefreshing;

    public void Initialize(List<UseableItem> useItems)
    {
        _useItems = useItems ?? new List<UseableItem>();

        Refresh();
    }

    public void Initialize(PlayerItemUse playerItemUse)
    {
        if (_playerItemUse != null)
        {
            _playerItemUse.InventoryChanged -= Refresh;
        }

        _playerItemUse = playerItemUse;
        if (_playerItemUse != null)
        {
            _playerItemUse.InventoryChanged += Refresh;
        }

        Initialize(_playerItemUse?.UseableItems);
    }

    public void OnUse()
    {
        Refresh();
    }

    private void OnDestroy()
    {
        if (_playerItemUse != null)
        {
            _playerItemUse.InventoryChanged -= Refresh;
        }

        foreach (var item in _useItemPanels)
        {
            if (item != null)
            {
                AddressableLoader.ReleaseInstance(item.gameObject);
            }
        }
    }

    private async void Refresh()
    {
        if (_isRefreshing)
        {
            return;
        }

        _isRefreshing = true;
        try
        {
            while (_useItemPanels.Count < _useItems.Count)
            {
                var panel = await GetUIPrefab(_useItems[_useItemPanels.Count], Container);
                _useItemPanels.Add(panel);
            }

            for (int i = 0; i < _useItemPanels.Count; i++)
            {
                if (i < _useItems.Count)
                {
                    _useItemPanels[i].Initialize(_useItems[i]);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception, this);
        }
        finally
        {
            _isRefreshing = false;
        }
    }

    private async Task<ItemPanel> GetUIPrefab(UseableItem item, Transform parent)
    {
        string address = item.AddressableKeyUI;
        if (string.IsNullOrEmpty(address) && item.Name == "Health Potion")
        {
            address = "HealPotionPanel";
        }

        if (string.IsNullOrEmpty(address))
        {
            return null;
        }

        var obj = await AddressableLoader.InstantiateAsync(address, parent);
        var panel = obj.GetComponent<ItemPanel>();
        if (panel == null)
        {
            AddressableLoader.ReleaseInstance(obj);
        }

        return panel;
    }
}