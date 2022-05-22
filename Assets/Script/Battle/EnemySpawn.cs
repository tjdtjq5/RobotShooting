using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyObj;
    public Transform startTrans;
    public Transform player;
    public EnemySO testSO;

    List<GameObject> spawnObjList = new List<GameObject>();

    public void Spawn(EnemySO _enemySO , SpawnPoint _spawnPoint)
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
        enemy.Spawn(_enemySO, player, new Vector2(Random.Range(-5.0f, 5) , startTrans.position.y), GetEndPositionY(_spawnPoint));
    }

    [Button("TestSpawn")]
    void TestSpawn(SpawnPoint _spawnPoint)
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
        enemy.Spawn(testSO, player, new Vector2(Random.Range(-5.0f, 5), startTrans.position.y), GetEndPositionY(_spawnPoint));
    }

    float GetEndPositionY(SpawnPoint _spawnPoint)
    {
        switch (_spawnPoint)
        {
            case SpawnPoint.최상:
                return Random.Range(6.0f,8);
            case SpawnPoint.상:
                return Random.Range(4.0f, 6);
            case SpawnPoint.중:
                return Random.Range(2.0f, 4);
            case SpawnPoint.하:
                return Random.Range(0.0f, 2);
        }
        return 0;
    }
}
public enum SpawnPoint
{
    최상,
    상,
    중,
    하
}