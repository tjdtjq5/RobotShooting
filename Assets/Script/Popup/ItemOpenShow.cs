using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOpenShow : Singleton<ItemOpenShow>
{
    public List<GameObject> objList = new List<GameObject>();

    public Image itemImg;
    public Text nameText, infoText;

    System.Action callback;
 
    public void Show(ItemSO _itemSO, System.Action _callback)
    {
        callback = _callback;
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetActive(true);
        }
        TextMessage.Instance.Show("아이템 개방 !!");
        itemImg.sprite = _itemSO.sprite;
        nameText.text = Language.Instance.GetScript(_itemSO.nameCode);
        nameText.font = Language.Instance.GetFont();
        infoText.text = Language.Instance.GetScript(_itemSO.scriptCode);
        infoText.font = Language.Instance.GetFont();
    }

    public void Close()
    {
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].SetActive(false);
        }

        callback();
    }
}
