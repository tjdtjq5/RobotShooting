using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : Singleton<BulletSpawn>
{
    [AssetList(Path = "SO/Bullet")] public List<BulletSO> bulletSOList = new List<BulletSO>();

    Dictionary<string, List<GameObject>> spawnObjList = new Dictionary<string, List<GameObject>>();
    public void Spawn(Transform _spawnTransform ,BulletSO _bulletSO, BulletType _bulletType, Transform _startTrans ,Transform _target, float _angle ,BulletHost _bulletHost, int _atk, int _cri, int _cridmg, int _tickDmg ,int _duration, int bulletSize)
    {
        float size = bulletSize / 1000f + 1;
        GameObject obj = null;
        if (spawnObjList.ContainsKey(_bulletSO.code))
        {
            for (int i = 0; i < spawnObjList[_bulletSO.code].Count; i++)
            {
                if (!spawnObjList[_bulletSO.code][i].activeSelf)
                {
                    obj = spawnObjList[_bulletSO.code][i];
                    obj.SetActive(true);
                    break;
                }
            }
        }
        else
        {
            spawnObjList.Add(_bulletSO.code, new List<GameObject>());
        }

        if (obj == null)
        {
            obj = Instantiate(_bulletSO.obj, this.transform);
            spawnObjList[_bulletSO.code].Add(obj);
        }
        obj.transform.localScale = new Vector3(size, size, size);
        obj.transform.position = _startTrans.position;
        obj.GetComponent<BulletObj>().Spawn(_spawnTransform, _bulletSO, _bulletType, _target, _angle, _bulletHost, _atk, _cri, _cridmg, _tickDmg, _duration, bulletSize);
    }
    public void AllDestroy()
    {
        foreach (var item in spawnObjList)
        {
            List<GameObject> objs = item.Value;
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].SetActive(false);
            }
        }
    }
}
public enum BulletHost
{
    플레이어,
    적
}