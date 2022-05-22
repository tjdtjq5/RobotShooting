using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Transform weaponTransform;
    public Transform bulletPos;
    public float searchRadius;

    public UserWeapon userWeapon;

    float a_time = 0;

    public void Hit(int dmg, int cri, int cridmg)
    {

    }

    public void Attack(BulletSO _bulletSO ,Transform _enemy)
    {
        a_time = 0;
        float angle = Function.Tool.GetAngle(this.transform.position, _enemy.position) - 90;
        BulletSpawn.Instance.Spawn(_bulletSO, _bulletSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어, 10, 10, 10, 1);
    }

    private void FixedUpdate()
    {
        a_time += Time.fixedDeltaTime;

        Transform enemy = Function.Tool.SearchCharacter(searchRadius, this.transform.position, "Enemy");
        if (enemy != null)
        {
            float angle = Function.Tool.GetAngle(bulletPos.position, enemy.position) - 90;
            weaponTransform.DOLocalRotate(new Vector3(0, 0, angle), 0.6f);

            BulletSO bulletSO = userWeapon.GetEqipWeapon().bulletSO;
            if (a_time >= bulletSO.atkspeed)
            {
                Attack(bulletSO, enemy);
            }
        }
    }
}
