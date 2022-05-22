using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Object/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public string code;
    public string nameCode;
    [PreviewField(100)] public Sprite sprite;
    public BulletSO bulletSO;
    public int coreCount;
}
