using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelect : Singleton<ItemSelect>
{
    public List<GameObject> objs = new List<GameObject>();
    public Transform list;
    public GameObject card;

    [Title("아이템")]
    [LabelText("일반 등급 아이템")] public List<ItemSO> nomal_itemSO;
    [LabelText("희귀 등급 아이템")] public List<ItemSO> rare_itemSO;
    [LabelText("영웅 등급 아이템")] public List<ItemSO> hero_itemSO;

    [HideInInspector] public int rarePercent_default = 700;
    [HideInInspector] public int heroPercent_default = 300;
    [HideInInspector] public int rarePercent;
    [HideInInspector] public int heroPercent;

    int cardCount_default = 3;
    [HideInInspector] public int cardCount;

    [HideInInspector] public int doubleCount = 0;

    [Title("탐색")]
    int q_default = 1;
    [HideInInspector] public int q_charge;
    [HideInInspector] public int q_count;
    public Transform q_card;
    public Text q_text;

    [Title("스크립트")]
    public BattleManager battleManager;

    public void BattleStart()
    {
        cardCount = cardCount_default;
        q_charge = q_default;
        rarePercent = rarePercent_default;
        heroPercent = heroPercent_default;
        doubleCount = 0;
    }

    public void Charge_Q()
    {
        q_count += q_charge;
    }

    public void Show()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(true);
        }
        Q_Setting();

        for (int i = 0; i < list.childCount - 1; i++)
        {
            list.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < cardCount; i++)
        {
            ItemSelectObj itemSelectObj = null;
            if (i < list.childCount - 1)
            {
                itemSelectObj = list.GetChild(i).GetComponent<ItemSelectObj>();
                itemSelectObj.gameObject.SetActive(true);
            }
            else
            {
                itemSelectObj = Instantiate(card, list).GetComponent<ItemSelectObj>();
            }

            itemSelectObj.Setting(GetItemSO(battleManager.waveIndex), () => {
                Close();
                battleManager.NextWave();
            });
        }
        q_card.SetSiblingIndex(list.childCount - 1);
    }

    void Close()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(false);
        }
    }

    void Q_Setting()
    {
        q_text.text = "탐색 포인트 + " + q_count;
    }

    public void OnClickReQ()
    {
        if (q_count <= 0)
        {
            TextMessage.Instance.Show("탐색 포인트가 없습니다");
            return;
        }
        q_count--;

        Show();
    }

    ItemSO GetItemSO(int wave)
    {
        wave++;

        bool isHero = (wave == 1 || wave % 5 == 0);
        bool isRare = false;

        if (isHero)
        {
            return hero_itemSO[Random.Range(0, hero_itemSO.Count)];
        }

        isHero = Function.GameInfo.IsCritical(heroPercent);
        isRare = Function.GameInfo.IsCritical(rarePercent);

        if (isHero)
        {
            return hero_itemSO[Random.Range(0, hero_itemSO.Count)];
        }

        if (isRare)
        {
            return rare_itemSO[Random.Range(0, rare_itemSO.Count)];
        }

        return nomal_itemSO[Random.Range(0, nomal_itemSO.Count)];
    }

    public void PushNomal()
    {
        ItemSO itemSO = nomal_itemSO[Random.Range(0, nomal_itemSO.Count)];
        UserItem.Instance.PushItem(itemSO);
    }
}
