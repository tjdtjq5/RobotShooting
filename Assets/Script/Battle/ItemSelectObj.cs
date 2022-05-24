﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectObj : MonoBehaviour
{
    public Image itemBoxImg;
    public Image itemImg;
    public Image infoBoxImg;
    public Text nameText;
    public Text infoText;
    public Button btn;

    public void Setting(ItemSO itemSO, System.Action _callback)
    {
        itemImg.sprite = itemSO.sprite;
        nameText.text = Language.Instance.GetScript(itemSO.nameCode) + " LV. " + UserItem.Instance.GetLevel(itemSO);
        infoText.text = itemSO.script;

        switch (itemSO.itemGrade)
        {
            case ItemGrade.일반:
                itemBoxImg.color = Color.white;
                nameText.color = new Color(72 / 255f, 61 / 255f, 55 / 255f, 1);
                infoBoxImg.color = Color.white;
                break;
            case ItemGrade.희귀:
                itemBoxImg.color = new Color(224f / 255f, 230 / 255f, 250 / 255f, 1);
                nameText.color = new Color(59 / 255f, 144 / 255f, 255 / 255f, 1);
                infoBoxImg.color = new Color(224f / 255f, 230 / 255f, 250 / 255f, 1);
                break;
            case ItemGrade.영웅:
                itemBoxImg.color = new Color(249f / 255f, 233 / 255f, 248 / 255f, 1);
                nameText.color = new Color(203 / 255f, 48 / 255f, 188 / 255f, 1);
                infoBoxImg.color = new Color(249f / 255f, 233 / 255f, 248 / 255f, 1);
                break;
        }

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => {
            UserItem.Instance.PushItem(itemSO);
            _callback();
        });
    }
}
