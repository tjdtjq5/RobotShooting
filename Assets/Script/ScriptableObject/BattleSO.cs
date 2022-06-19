using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleSO_0", menuName = "Scriptable Object/BattleSO")]
public class BattleSO : ScriptableObject
{
    public string code;
    public string nameCode;
    public List<WaveSO> waveSOs = new List<WaveSO>();
    [LabelText ("공격력 버프")] public int atkBuff;
    [LabelText("공격속도 버프 (- 수치 입력)")] [MaxValue(0)]public float atkspeedBuff;
    [LabelText("체력 버프")] public int hpBuff;
    public int rewardCoreCount;
    public string unLockBattleCode;
}
