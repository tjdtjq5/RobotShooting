using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : Singleton<Electric>
{
    public BulletSO electricBulletSO_1, electricBulletSO_2;

    public void ElectricSpawn_1(Transform _startTrans ,int _count, int _atk, int _cri , int cridmg)
    {
        if (Player.Instance.isElectro)
        {
            _count *= 2;
        }
        int angle = 360 / _count;
        for (int i = 0; i < _count; i++)
        {
            BulletSpawn.Instance.Spawn(Player.Instance.transform ,electricBulletSO_1, BulletType.전기_1, _startTrans, this.transform, angle * i,
                BulletHost.플레이어, _atk, _cri, cridmg, UserAbility.Instance.GetAbility(Ability.손상피해), 0, 1 );
        }
    }

    public void ElectricSpawn_2(Transform _startTrans, int _count, int _atk, int _cri, int cridmg)
    {
        if (Player.Instance.isElectro)
        {
            _count *= 2;
        }
        int angle = 360 / _count;
        for (int i = 0; i < _count; i++)
        {
            BulletSpawn.Instance.Spawn(Player.Instance.transform , electricBulletSO_2, BulletType.전기_2, _startTrans, this.transform, angle * i,
                BulletHost.플레이어, _atk, _cri, cridmg, UserAbility.Instance.GetAbility(Ability.손상피해), 0, 1);
        }
    }
}
