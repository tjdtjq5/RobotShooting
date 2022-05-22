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

    public List<BoxCollider2D> bounds = new List<BoxCollider2D>();


    public List<GameObject> spawnObjList = new List<GameObject>();

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
}
