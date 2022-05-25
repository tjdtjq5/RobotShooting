using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UserAbility : Singleton<UserAbility>
{
    [SerializeField] List<AbilityData> defaultAbility = new List<AbilityData>();

    Dictionary<Ability, int> totalAbility = new Dictionary<Ability, int>();

    public Upgrade upgrade;

    // 디폴트 + 업그레이드
    public void BattleSetting()
    {
        totalAbility.Clear();
        for (int i = 0; i < System.Enum.GetValues(typeof(Ability)).Length; i++)
        {
            Ability ability = (Ability)i;
            int count = 0;
            for (int j = 0; j < defaultAbility.Count; j++)
            {
                if (ability == defaultAbility[j].ability)
                {
                    count += defaultAbility[j].count;
                }
            }

            for (int j = 0; j < upgrade.upgradeSO.upgradeDatas.Count; j++)
            {
                if (ability == upgrade.upgradeSO.upgradeDatas[j].abilityData.ability)
                {
                    string upgradeCode = upgrade.upgradeSO.upgradeDatas[j].code;
                    count += (upgrade.GetUpgradeCount(upgradeCode) * upgrade.upgradeSO.upgradeDatas[j].abilityData.count); 
                }
            }

            totalAbility.Add(ability, count);
        }
    }
    public int GetAbility(Ability _ability)
    {
        return totalAbility[_ability];
    }
    public void BuffAbility(AbilityData _abilityData)
    {
        totalAbility[_abilityData.ability] += _abilityData.count;

        if (totalAbility[_abilityData.ability] < 0)
        {
            totalAbility[_abilityData.ability] = 0;
        }
    }
}

public enum Ability
{
    발사체공격력,
    공격속도,
    체력,
    방어력,
    이동속도,
    치명타확률_최대1000,
    발사체내구도,
    치명데미지_최대1000,
    코어드랍률_최대1000,
    적수에따라HP회복_최대1000,
    HP흡수_최대1000,
    HP50이하일때공격력증가_최대1000,
    멈춰있을때공격속도증가_최대1000,
    손상피해,
    회피_최대1000,
    웨이브마다최대체력상승,
    발사체크기,
    피해를입은후무적
}
[System.Serializable]
public class AbilityData
{
    public Ability ability;
    [MinValue(0)] public int count;

    public AbilityData()
    {

    }
    public AbilityData(Ability _ability, int _count)
    {
        this.ability = _ability;
        this.count = _count;
    }
}
