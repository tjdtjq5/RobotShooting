using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : Singleton<EnemySpawn>
{
    public GameObject enemyObj;
    public Transform startTrans;
    public Transform player;

    public List<BoxCollider2D> bounds = new List<BoxCollider2D>();


    public List<GameObject> spawnObjList = new List<GameObject>();

    public void Spawn(EnemySO _enemySO , BattleSO _battleSO ,SpawnPoint _spawnPoint)
    {
        GameObject obj = null;
        for (int i = 0; i < spawnObjList.Count; i++)
        {
            if (!spawnObjList[i].activeSelf)
            {
                obj = spawnObjList[i];
                obj.SetActive(true);
                break;
            }
        }
        if (obj == null)
        {
            obj = Instantiate(enemyObj, this.transform);
            spawnObjList.Add(obj);
        }

        EnemyObj enemy = obj.GetComponent<EnemyObj>();
        float rx = Random.Range(-5.0f, 5);
        float ey = GetEndPositionY(_spawnPoint);
        enemy.Spawn(_enemySO, _battleSO, player, new Vector2(rx , startTrans.position.y), ey);
    }
    float GetEndPositionY(SpawnPoint _spawnPoint)
    {
        Vector2 minBound = bounds[(int)_spawnPoint].bounds.min;
        Vector2 maxBound = bounds[(int)_spawnPoint].bounds.max;

        return Random.Range(minBound.y, maxBound.y);
    }
    public void AllDestroy()
    {
        for (int i = 0; i < spawnObjList.Count; i++)
        {
            spawnObjList[i].SetActive(false);
        }
    }
    public int EnemyCount()
    {
        int count = 0;
        for (int i = 0; i < spawnObjList.Count; i++)
        {
            if (spawnObjList[i].activeSelf)
            {
                count++;
            }
        }
        return count;
    }
}
