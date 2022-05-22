using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO_0", menuName = "Scriptable Object/EnemySO")]
public class EnemySO : ScriptableObject
{
    public string code;
    public string nameCode;
    [PreviewField(100)] public Sprite sprite;
    [MinValue(1)] public int atk, hp , atkspeed, movespeed;
    public BulletSO bulletSO;
    public BulletHost bulletHost;
    public EnemyMovePattern enemyMovePattern;
    [MinValue(0)] public float spawnTime;
    public Vector2 colliderSize;

    private void Awake()
    {
        code = "Enemy_0";
        nameCode = "Enemy_name_0";
    }
}
public enum EnemyMovePattern
{
    없음,
    반대방향,
    정방향
}
