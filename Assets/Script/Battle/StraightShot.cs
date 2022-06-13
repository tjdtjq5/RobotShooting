using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightShot : Singleton<StraightShot>
{
    public Transform player;
    public BulletSO bulletSO;

    IEnumerator shotCoroutine;
    float atkspeed = 0.1f;

    [HideInInspector] public bool isUpgradeItemOpen = false;

    public void Shot()
    {
        atkspeed = bulletSO.atkspeed;
        if (shotCoroutine != null)
        {
            StopCoroutine(shotCoroutine);
        }
        shotCoroutine = ShotCoroutine();
        StartCoroutine(shotCoroutine);
    }

    IEnumerator ShotCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(atkspeed);
        while (true)
        {
            yield return waitTime;

            float angle = 0;
            int atk = isUpgradeItemOpen ? bulletSO.atk * 3 : bulletSO.atk;
            BulletSpawn.Instance.Spawn(this.transform, bulletSO, bulletSO.bulletType, player, null, angle, BulletHost.플레이어,
                atk,
                UserAbility.Instance.GetAbility(Ability.치명데미지_최대1000)
                , UserAbility.Instance.GetAbility(Ability.치명데미지_최대1000)
                , UserAbility.Instance.GetAbility(Ability.손상피해)
                , UserAbility.Instance.GetAbility(Ability.발사체내구도)
                , UserAbility.Instance.GetAbility(Ability.발사체크기));
        }
    }

    public void Upgrade(int _value)
    {
        float value = _value / 1000f;
        atkspeed -= value;
        if (atkspeed < 0.1f)
        {
            atkspeed = 0.1f;
        }
    }

    public void End()
    {
        if (shotCoroutine != null)
        {
            StopCoroutine(shotCoroutine);
        }

        isUpgradeItemOpen = false;
    }
}
