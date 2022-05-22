using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSO", menuName = "Scriptable Object/UpgradeSO")]
public class UpgradeSO : ScriptableObject
{
    public List<UpgradeData> upgradeDatas = new List<UpgradeData>();
}
[System.Serializable]
public class UpgradeData
{
    public string code;
    public string nameTextCode;
    [PreviewField(100)] public Sprite icon;
    public AbilityData abilityData;
    [MinValue(0)] public int maxCount;
}