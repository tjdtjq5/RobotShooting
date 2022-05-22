using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleSO_0", menuName = "Scriptable Object/BattleSO")]
public class BattleSO : ScriptableObject
{
    public string code;
    public string nameCode;
    public List<WaveSO> waveSOs = new List<WaveSO>();
    public List<AbilityData> enemyBuffs = new List<AbilityData>();
    public int rewardCoreCount;
    public string unLockBattleCode;
}
