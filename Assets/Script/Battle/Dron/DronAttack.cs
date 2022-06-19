using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DronAttack : MonoBehaviour
{
    private void OnEnable()
    {
        AttackMode();
    }

    Sequence atkSequence;
    float searchRadius = 20;

    void AttackMode()
    {
        if (atkSequence != null)
        {
            atkSequence.Kill();
        }
        atkSequence = DOTween.Sequence();

        BulletSO bulletSO = UserWeapon.Instance.GetEqipWeapon().bulletSO;
        if (bulletSO.bulletType == BulletType.위성레이저)
        {
            bulletSO = BulletSpawn.Instance.bulletSOList[0];
        }
        float atkspeed = bulletSO.atkspeed / (UserAbility.Instance.GetAbility(Ability.공격속도) / 1000f + 1) * 2;
        if (Player.Instance.isMultiGubter)
        {
            atkspeed /= 2f;
        }
        atkSequence.InsertCallback(atkspeed, () => {
            Transform enemy = Function.Tool.SearchCharacter(searchRadius, this.transform.position, "Enemy");
            float angle = Function.Tool.GetAngle(this.transform.position, enemy.position) - 90;
            BulletSpawn.Instance.Spawn(Player.Instance.transform, bulletSO, bulletSO.bulletType, this.transform, enemy, angle, BulletHost.플레이어,
                  UserAbility.Instance.GetAbility(Ability.발사체공격력) / 2,
                 UserAbility.Instance.GetAbility(Ability.치명타확률_최대1000)
                 , UserAbility.Instance.GetAbility(Ability.치명데미지_최대1000),
                   0
                 , UserAbility.Instance.GetAbility(Ability.발사체내구도)
                 , UserAbility.Instance.GetAbility(Ability.발사체크기));
        }).SetLoops(-1 , LoopType.Incremental).Play();
    }

    private void OnDisable()
    {
        if (atkSequence != null)
        {
            atkSequence.Kill();
        }
    }
}
