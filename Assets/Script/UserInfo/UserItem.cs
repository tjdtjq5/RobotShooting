using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserItem : Singleton<UserItem>
{
    Dictionary<ItemSO, int> userItemDics = new Dictionary<ItemSO, int>();
    public void BattleSetting()
    {
        userItemDics.Clear();
    }
    public void PushItem(ItemSO _itemSO)
    {
        if (userItemDics.ContainsKey(_itemSO))
        {
            // 레벨업
            userItemDics[_itemSO]++;
        }
        else
        {
            userItemDics.Add(_itemSO, 1);
        }

        // 추가 아이템 
        if (_itemSO.plusItem != null && userItemDics[_itemSO] == _itemSO.plusItemLevel)
        {
            PushItem(_itemSO.plusItem);
        }
    }
    public int GetLevel(ItemSO _itemSO)
    {
        return (userItemDics.ContainsKey(_itemSO)) ? userItemDics[_itemSO] : 0;
    }
}
