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
    Sequence satelliteSequence_2;

    [Button("StatelliteStart")]
    public void StatelliteStart_1(int _atk, int _atkspeed, int _satelliteCount)
    {
        if (satelliteSequence_1 != null)
        {
            satelliteSequence_1.Kill();
        }
        satelliteSequence_1 = DOTween.Sequence();

        int atk = _atk;
        float atkspeed = bulletSO.atkspeed + bulletSO.atkspeed * (_atkspeed / 1000f);
        int satelliteCount = _satelliteCount;
        Debug.Log(atkspeed);

        satelliteSequence_1.InsertCallback(atkspeed, () => {

            for (int i = 0; i < enemySpawn.spawnObjList.Count; i++)
            {
                if (enemySpawn.spawnObjList[i].activeSelf && satelliteCount >0)
                {
                    satelliteCount--;

                    float angle = Function.Tool.GetAngle(this.transform.position, enemySpawn.spawnObjList[i].transform.position) - 90;
                    BulletSpawn.Instance.Spawn(Player.Instance.transform, bulletSO, bulletSO.bulletType, this.transform,
                       enemySpawn.spawnObjList[i].transform, angle, BulletHost.플레이어,
                       atk, 0, 0,0, bulletSO.bulletDuration, 1);
                }
            }

            satelliteCount = _satelliteCount;

        }).SetLoops(-1, LoopType.Incremental);
        satelliteSequence_1.Play();
    }
    public void StatelliteStart_2(int _atk, int _atkspeed, int _satelliteCount)
    {
        if (satelliteSequence_2 != null)
        {
            satelliteSequence_2.Kill();
        }
        satelliteSequence_2 = DOTween.Sequence();

        int atk = _atk;
        float atkspeed = bulletSO.atkspeed + bulletSO.atkspeed * (_atkspeed / 1000f);
        int satelliteCount = _satelliteCount;
        Debug.Log(atkspeed);

        satelliteSequence_2.InsertCallback(atkspeed, () => {

            for (int i = 0; i < enemySpawn.spawnObjList.Count; i++)
            {
                if (enemySpawn.spawnObjList[i].activeSelf && satelliteCount > 0)
                {
                    satelliteCount--;

                    float angle = Function.Tool.GetAngle(this.transform.position, enemySpawn.spawnObjList[i].transform.position) - 90;
                    BulletSpawn.Instance.Spawn(Player.Instance.transform, bulletSO, bulletSO.bulletType, this.transform,
                       enemySpawn.spawnObjList[i].transform, angle, BulletHost.플레이어,
                       atk, 0, 0,0 ,bulletSO.bulletDuration, 1);
                }
            }

            satelliteCount = _satelliteCount;

        }).SetLoops(-1, LoopType.Incremental);
        satelliteSequence_2.Play();
    }
    public void StatelliteStop()
    {
        if (satelliteSequence_1 != null)
        {
            satelliteSequence_1.Kill();
        }
        if (satelliteSequence_2 != null)
        {
            satelliteSequence_2.Kill();
        }
    }
}
