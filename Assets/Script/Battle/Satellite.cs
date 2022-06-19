using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Satellite : Singleton<Satellite>
{
    public EnemySpawn enemySpawn;
    public BulletSO bulletSO;

    Sequence satelliteSequence_1;

    public List<Transform> posList = new List<Transform>();

    int satelliteCount;

    [Button("StatelliteStart")]
    public void StatelliteStart_1(int _atk, float _atkspeed, int _satelliteCount)
    {
        if (satelliteSequence_1 != null)
        {
            satelliteSequence_1.Kill();
        }
        satelliteSequence_1 = DOTween.Sequence();

        if (satelliteCount < _satelliteCount)
        {
            satelliteCount = _satelliteCount;
        }

        int atk = _atk;
        if (_atkspeed <= 0.1f)
        {
            _atkspeed = 0.1f;
        }

        satelliteSequence_1.InsertCallback(_atkspeed, () => {

            bool flag = false;
            for (int i = 0; i < enemySpawn.spawnObjList.Count; i++)
            {
                if (enemySpawn.spawnObjList[i].activeSelf)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                for (int i = 0; i < satelliteCount; i++)
                {
                    BulletSpawn.Instance.Spawn(Player.Instance.transform, bulletSO, bulletSO.bulletType, this.transform,
                       posList[Random.Range(0, posList.Count - 1)], 0, BulletHost.플레이어,
                       atk, 0, 0, UserAbility.Instance.GetAbility(Ability.손상피해), bulletSO.bulletDuration, 1);
                }
            }

        }).SetLoops(-1, LoopType.Incremental);
        satelliteSequence_1.Play();
    }
 
    public void StatelliteStop()
    {
        if (satelliteSequence_1 != null)
        {
            satelliteSequence_1.Kill();
        }
        satelliteCount = 0;
    }
}
