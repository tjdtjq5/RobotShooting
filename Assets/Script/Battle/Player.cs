using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public Transform weaponTransform;
    public Transform bulletPos;
    public float searchRadius;

    public Slider hp_scrollbar;
    public Text hp_text;

    public UserWeapon userWeapon;
    public UserAbility userAbility;
    public BattleManager battleManager;
    public JoyStick joyStick;
    public AniManager aniManager;
    public UserMove userMove;

    public bool isBattle = false;
    float a_time = 0;
    int hp = 0;

    private void Start()
    {
        this.gameObject.tag = "Player";
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void BattleSetting()
    {
        isBattle = true;
        userAbility.BattleSetting();

        hp = userAbility.GetAbility(Ability.체력);

        HP_Setting();
    }

    public void Hit(int dmg, int cri, int cridmg)
    {
        if (!isBattle)
            return;

        Vector2 pos = this.transform.position;
        pos = new Vector2(pos.x, pos.y + 1.5f);
        DmgSpawn.Instance.Spawn(pos, dmg.ToString());

        hp -= dmg;
        HP_Setting();

        if (hp <= 0)
        {
            hp_text.text = "0 / " + userAbility.GetAbility(Ability.체력);
            Death();
        }
    }

    void HP_Setting()
    {
        hp_scrollbar.value = (float)hp / userAbility.GetAbility(Ability.체력);
        hp_text.text = hp + " / " + userAbility.GetAbility(Ability.체력);
    }

    void Death()
    {
        battleManager.BattleFailure();
    }

    public void BattleEnd(System.Action _callback)
    {
        isBattle = false;
        joyStick.DragEnd();

        if ((this.transform.position.x > 0))
        {
            aniManager.PlayAnimation(userMove.walkL, false);
        }
        else
        {
            aniManager.PlayAnimation(userMove.walkR, false);
        }


        this.transform.DOMove(new Vector2(0, -4.38f), 1.5f).OnComplete(()=> {
            aniManager.PlayAnimation(userMove.waitAniKey, false);
            _callback();
        });
    }

    public void Attack(BulletSO _bulletSO ,Transform _enemy)
    {
        if (!isBattle)
            return;

        a_time = 0;
        float angle = Function.Tool.GetAngle(this.transform.position, _enemy.position) - 90;
        BulletSpawn.Instance.Spawn(_bulletSO, _bulletSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어, 
            userAbility.GetAbility(Ability.발사체공격력),
            userAbility.GetAbility(Ability.치명타확률)
            , userAbility.GetAbility(Ability.치명데미지)
            , userAbility.GetAbility(Ability.발사체내구도));
    }



    private void FixedUpdate()
    {
        if (!isBattle)
            return;

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
