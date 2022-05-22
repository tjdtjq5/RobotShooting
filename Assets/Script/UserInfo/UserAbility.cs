using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UserAbility : MonoBehaviour
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
    치명타확률,
    발사체내구도,
    코어드랍확률,
    탐색,
    추가공격력,
    치명데미지
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
