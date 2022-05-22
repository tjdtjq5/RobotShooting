using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO_0", menuName = "Scriptable Object/BulletSO")]
public class BulletSO : ScriptableObject
{
    public string code;
    public string nameCode;
    public GameObject obj;
    public int atk;
    public int atkPlus;
    public int atkspeed;
    public int bulletDuration;
    [MinValue(1)] public float movespeed;
    public BulletType bulletType;

    private void Awake()
    {
        code = "bullet_0";
        nameCode = "bullet_name_0";
    }
}
public enum BulletType
{ 
    기본,
    유도,
    다발,
    관통,
    위성레이저,
    전기_1,
    전기_2
}