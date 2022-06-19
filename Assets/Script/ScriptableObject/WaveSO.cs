using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO_0", menuName = "Scriptable Object/WaveSO")]
public class WaveSO : ScriptableObject
{
    public List<WaveEnemy> waveEnemies = new List<WaveEnemy>();
}

[System.Serializable]
public class WaveEnemy
{
    public EnemySO enemySO;
    public SpawnPoint spawnPoint;
    public float spawnTime;
}
public enum SpawnPoint
{
    최상,
    상,
    중,
    하
}