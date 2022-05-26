using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserItem : Singleton<UserItem>
{
    Dictionary<ItemSO, int> userItemDics = new Dictionary<ItemSO, int>();
    public void BattleSetting()
    {
        userItemDics.Clear();
        UI_Setting();
    }
    public void PushItem(ItemSO _itemSO)
    {
        if (userItemDics.ContainsKey(_itemSO))
        {
            // 레벨업
            userItemDics[_itemSO]++;
            Ability_LevelUp(_itemSO);
        }
        else
        {
            userItemDics.Add(_itemSO, 1);
            Ability_Setting(_itemSO);
        }

        // 추가 아이템 
        if (_itemSO.plusItem != null && userItemDics[_itemSO] == _itemSO.plusItemLevel)
        {
            PushItem(_itemSO.plusItem);
        }

        UI_Setting();
    }
    public int GetLevel(ItemSO _itemSO)
    {
        return (userItemDics.ContainsKey(_itemSO)) ? userItemDics[_itemSO] : 0;
    }

    // UI
    public GameObject cardPrepab;
    public Transform list;
    void UI_Setting()
    {
        for (int i = 0; i < list.childCount; i++)
        {
            list.GetChild(i).gameObject.SetActive(false);
        }

        Transform card = null;

        var itemSOList = new List<ItemSO>(userItemDics.Keys);
        var levelList = new List<int>(userItemDics.Values);

        for (int i = 0; i < userItemDics.Count; i++)
        {
            if (i < list.childCount)
            {
                card = list.GetChild(i);
                card.gameObject.SetActive(true);
            }
            else
            {
                card = Instantiate(cardPrepab, list).transform;
            }

            ItemSO itemSO = itemSOList[i];
            int level = levelList[i];

            Image iconImg = card.Find("icon").GetComponent<Image>();
            Text levelText = card.Find("level").GetComponent<Text>();

            iconImg.sprite = itemSO.sprite;
            levelText.text = level.ToString();
        }
    }


    // 능력치 적용
    void Ability_Setting(ItemSO _itemSO)
    {
        switch (_itemSO.itemType)
        {
            case ItemType.부스터:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.이동속도, _itemSO.boosterCount));
                break;
            case ItemType.탄환:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체공격력, _itemSO.bulletCount));
                break;
            case ItemType.재장전:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.공격속도, _itemSO.reloadCount));
                break;
            case ItemType.에너지:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.체력, _itemSO.energyCount));
                Player.Instance.HP_Setting();
                break;
            case ItemType.코어드랍:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.코어드랍률_최대1000, _itemSO.coredropCount));
                break;
            case ItemType.에너지드링크:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.웨이브마다최대체력상승, _itemSO.energyDringCount));
                break;
            case ItemType.아머:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.방어력, _itemSO.armorCount));
                break;
            case ItemType.센서:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.치명타확률_최대1000, _itemSO.senserCount));
                break;
            case ItemType.탄환저항:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체내구도, _itemSO.bulletStabilityCount));
                break;
            case ItemType.쇠구슬:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체크기, _itemSO.ironballCount));
                break;
            case ItemType.열처리:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.피해를입은후무적, _itemSO.heatTreatmentCount));
                break;
            case ItemType.충전:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.적수에따라HP회복_최대1000, _itemSO.chargeCount));
                break;
            case ItemType.에너지흡수:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.HP흡수_최대1000, _itemSO.energeAbsorptionCount));
                break;
            case ItemType.주사위:
                ItemSelect.Instance.rarePercent += _itemSO.diceCount;
                break;
            case ItemType.과열:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.HP50이하일때공격력증가_최대1000, _itemSO.overHeatCount));
                break;
            case ItemType.인공위성:
                if (_itemSO.itemGrade == ItemGrade.영웅)
                {
                    Satellite.Instance.StatelliteStart_2(0, 0, 6);
                }
                else
                {
                    Satellite.Instance.StatelliteStart_1(0, 0, 6);
                }
                break;
            case ItemType.정밀사격:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.치명데미지_최대1000, _itemSO.precisionCount));
                break;
            case ItemType.축소장치:
                float size = 1 - (_itemSO.smallCount / 1000f);
                Player.Instance.SizeSetting(size);
                break;
            case ItemType.전기폭발:
                Player.Instance.isElectric = true;
                Player.Instance.electricCount = _itemSO.electricExplosionCount;
                break;
            case ItemType.드론:
                DronSpawn.Instance.Spawn();
                break;
            case ItemType.설계도:
                ItemSelect.Instance.cardCount++;
                break;
            case ItemType.방어막:
                Player.Instance.Barrior_Start(_itemSO.barrierTime);
                break;
            case ItemType.머신건:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.멈춰있을때공격속도증가_최대1000, _itemSO.machineGun));
                break;
            case ItemType.복사기:
                ItemSelect.Instance.doubleCount += _itemSO.copy;
                break;
            case ItemType.혼합기름:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.손상피해, _itemSO.mixedOil));
                break;
            case ItemType.니트로:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.회피_최대1000, _itemSO.nitro));
                break;
            case ItemType.매그넘44:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체공격력, _itemSO.magnum));
                break;
            case ItemType.수퍼로봇:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.체력, _itemSO.superRobot_maxhp));
                Player.Instance.HP_Setting();
                Player.Instance.DoubleSize();
                break;
            case ItemType.수집가:
                Player.Instance.isCollection = true;
                break;
            case ItemType.레드불:
                Player.Instance.hp = UserAbility.Instance.GetAbility(Ability.체력);
                Player.Instance.HP_Setting();
                break;
            case ItemType.가시갑옷:

                break;
            case ItemType.레이저:
                break;
            case ItemType.자이로스코프:
                break;
            case ItemType.팩맨:
                break;
            case ItemType.와이퍼:
                break;
            case ItemType.침착:
                break;
            case ItemType.전기장:
                break;
            case ItemType.스펀지:
                break;
            case ItemType.복주머니:
                break;
            case ItemType.재가동:
                break;
            case ItemType.스타링크:
                break;
            case ItemType.하이눈:
                break;
            case ItemType.포켓봇:
                break;
            case ItemType.일렉트로:
                break;
            case ItemType.멀티곱터:
                break;
            case ItemType.오토캐드:
                break;
            case ItemType.반사장치:
                break;
            case ItemType.벙커:
                break;
            case ItemType.자동생성:
                break;
            case ItemType.스포일러:
                break;
        }
    }
    void Ability_LevelUp(ItemSO _itemSO)
    {
        switch (_itemSO.itemType)
        {
            case ItemType.부스터:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.이동속도, _itemSO.boosterCount_levelup));
                break;
            case ItemType.탄환:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체공격력, _itemSO.bulletCount_levelup));
                break;
            case ItemType.재장전:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.공격속도, _itemSO.reloadCount_levelup));
                break;
            case ItemType.에너지:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.체력, _itemSO.energyCount_levelup));
                Player.Instance.HP_Setting();
                break;
            case ItemType.코어드랍:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.코어드랍률_최대1000, _itemSO.coredropCount_levelup));
                break;
            case ItemType.에너지드링크:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.웨이브마다최대체력상승, _itemSO.energyDringCount_levelup));
                break;
            case ItemType.아머:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.방어력, _itemSO.armorCount_levelup));
                break;
            case ItemType.센서:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.치명타확률_최대1000, _itemSO.senserCount_levelup));
                break;
            case ItemType.탄환저항:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체내구도, _itemSO.bulletStabilityCount_levelup));
                break;
            case ItemType.쇠구슬:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.발사체크기, _itemSO.ironballCount_levelup));
                break;
            case ItemType.열처리:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.피해를입은후무적, _itemSO.heatTreatmentCount_levelup));
                break;
            case ItemType.충전:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.적수에따라HP회복_최대1000, _itemSO.chargeCount_levelup));
                break;
            case ItemType.에너지흡수:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.HP흡수_최대1000, _itemSO.energeAbsorptionCount_levelup));
                break;
            case ItemType.주사위:
                ItemSelect.Instance.rarePercent += _itemSO.diceCount_levelup;
                break;
            case ItemType.과열:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.HP50이하일때공격력증가_최대1000, _itemSO.overHeatCount_levelup));
                break;
            case ItemType.인공위성:
                if (_itemSO.itemGrade == ItemGrade.영웅)
                {
                    Satellite.Instance.StatelliteStart_2(_itemSO.satelliteAtkCount_levelup * (GetLevel(_itemSO) - 1), _itemSO.satelliteAtkspeedCount_levelup * (GetLevel(_itemSO) - 1), 6);
                }
                else
                {
                    Satellite.Instance.StatelliteStart_1(_itemSO.satelliteAtkCount_levelup * (GetLevel(_itemSO) - 1), _itemSO.satelliteAtkspeedCount_levelup * (GetLevel(_itemSO) - 1), 3);
                }
                break;
            case ItemType.정밀사격:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.치명데미지_최대1000, _itemSO.precisionCount_levelup));
                break;
            case ItemType.축소장치:
                float size = 1 - ((_itemSO.smallCount + _itemSO.smallCount_levelup * (GetLevel(_itemSO) - 1)) / 1000f);
                Player.Instance.SizeSetting(size);
                break;
            case ItemType.전기폭발:
                Player.Instance.electricCount += _itemSO.electricExplosionCount_levelup;
                break;
            case ItemType.드론:
                DronSpawn.Instance.Spawn();
                break;
            case ItemType.설계도:
                ItemSelect.Instance.cardCount++;
                break;
            case ItemType.방어막:
                Player.Instance.Barrior_Start(_itemSO.barrierTime - (_itemSO.barrierTime_levelup * (GetLevel(_itemSO) - 1)));
                break;
            case ItemType.머신건:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.멈춰있을때공격속도증가_최대1000, _itemSO.machineGun_levelup));
                break;
            case ItemType.복사기:
                ItemSelect.Instance.doubleCount += _itemSO.copy;
                break;
            case ItemType.혼합기름:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.손상피해, _itemSO.mixedOil_levelup));
                break;
            case ItemType.니트로:
                UserAbility.Instance.BuffAbility(new AbilityData(Ability.회피_최대1000, _itemSO.nitro));
                break;
            case ItemType.매그넘44:
                break;
            case ItemType.수퍼로봇:
                break;
            case ItemType.수집가:
                break;
            case ItemType.레드불:
                break;
            case ItemType.가시갑옷:
                break;
            case ItemType.레이저:
                break;
            case ItemType.자이로스코프:
                break;
            case ItemType.팩맨:
                break;
            case ItemType.와이퍼:
                break;
            case ItemType.침착:
                break;
            case ItemType.전기장:
                break;
            case ItemType.스펀지:
                break;
            case ItemType.복주머니:
                break;
            case ItemType.재가동:
                break;
            case ItemType.스타링크:
                break;
            case ItemType.하이눈:
                break;
            case ItemType.포켓봇:
                break;
            case ItemType.일렉트로:
                break;
            case ItemType.멀티곱터:
                break;
            case ItemType.오토캐드:
                break;
            case ItemType.반사장치:
                break;
            case ItemType.벙커:
                break;
            case ItemType.자동생성:
                break;
            case ItemType.스포일러:
                break;
            default:
                break;
        }
    }
}
