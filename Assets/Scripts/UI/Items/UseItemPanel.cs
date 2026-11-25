using System.Collections.Generic;
using UnityEngine;

public class UseItemPanel : MonoBehaviour
{
    public ItemPanel Prefab;
    public Transform Container;

    private List<ItemPanel> _items = new();
    private List<UseableItem> _sourceItems = new();
    private PlayerItemUse _playerItemUse;

    public void Initialize(List<UseableItem> items)
    {
        _sourceItems = items ?? new List<UseableItem>();

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
    }

    private void Refresh()
    {
        if (_sourceItems.Count == 0)
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    item.gameObject.SetActive(false);
                }
            }
            return;
        }

        var firstItem = _sourceItems[0];

        Transform container = Container != null ? Container : transform;
        while (_items.Count < _sourceItems.Count)
        {
            ItemPanel panel;
            if (Prefab != null)
            {
                panel = Instantiate(Prefab, container);
            }
            else
            {
                panel = container.GetComponentInChildren<ItemPanel>(true);
                if (panel == null)
                {
                    break;
                }
            }
            _items.Add(panel);
        }

        for (int i = 0; i < _items.Count; i++)
        {
            if (i < _sourceItems.Count)
            {
                _items[i].Initialize(_sourceItems[i]);
            }
            else
            {
                _items[i].gameObject.SetActive(false);
            }
        }
    }
}