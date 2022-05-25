using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Singleton<Player>
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
    public EnemySpawn enemySpawn;

    public bool isBattle = false;
    float a_time = 0;
    int hp = 0;

    // 무적
    bool isInvince;

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

        HPRecovery_1Start();
    }

    public void Hit(int dmg, int cri, int cridmg)
    {
        if (!isBattle)
            return;

        if (isInvince)
        {
            // 무적
            return;
        }

        Invince(userAbility.GetAbility(Ability.피해를입은후무적) / 1000f);

        Vector2 pos = this.transform.position;
        pos = new Vector2(pos.x, pos.y + 1.5f);
        DmgSpawn.Instance.Spawn(pos, dmg.ToString());

        bool isCri = Function.GameInfo.IsCritical(cri);
        dmg = isCri ? (int)(dmg * (cridmg / 1000f)) : dmg;
        hp -= dmg;
        HP_Setting();

        if (hp <= 0)
        {
            hp_text.text = "0 / " + userAbility.GetAbility(Ability.체력);
            Death();
        }
    }

    public void HP_Setting()
    {
        hp_scrollbar.value = (float)hp / userAbility.GetAbility(Ability.체력);
        hp_text.text = hp + " / " + userAbility.GetAbility(Ability.체력);
    }

    public void HP_Recovery(int _hp)
    {
        if (!isBattle)
        {
            return;
        }

        hp += _hp;

        HP_Setting();
    }

    void Death()
    {
        battleManager.BattleFailure();
    }

    public void BattleEnd(System.Action _callback)
    {
        isBattle = false;
        joyStick.DragEnd();
        HPRecovery_1Stop();
        SizeSetting(1);

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

        float atk = userAbility.GetAbility(Ability.발사체공격력);
        float hpPercent = ((float)hp / userAbility.GetAbility(Ability.체력));
        if (hpPercent <= 0.5f)
        {
            float percent = (0.5f - hpPercent) * 100;
            atk *= 1 + (percent * (userAbility.GetAbility(Ability.HP50이하일때공격력증가_최대1000) / 1000f));
        }

        float angle = Function.Tool.GetAngle(this.transform.position, _enemy.position) - 90;
        BulletSpawn.Instance.Spawn(_bulletSO, _bulletSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어,
            (int)atk,
            userAbility.GetAbility(Ability.치명데미지_최대1000)
            , userAbility.GetAbility(Ability.치명데미지_최대1000)
            , userAbility.GetAbility(Ability.발사체내구도)
            , userAbility.GetAbility(Ability.발사체크기));
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
            if (a_time >= bulletSO.atkspeed / (userAbility.GetAbility(Ability.공격속도) / 1000f + 1))
            {
                Attack(bulletSO, enemy);
            }
        }
    }

    //무적
    Sequence invinceSequence;
    void Invince(float time)
    {
        if (invinceSequence != null)
        {
            invinceSequence.Kill();
        }

        invinceSequence = DOTween.Sequence();

        isInvince = true;

        invinceSequence.InsertCallback(time, () => isInvince = false);
    }

    // 적 수에 비례하여 체력회복
    Sequence hprecoverySequence_1;
    void HPRecovery_1Start()
    {
        if (hprecoverySequence_1 != null)
        {
            hprecoverySequence_1.Kill();
        }

        hprecoverySequence_1 = DOTween.Sequence();

        hprecoverySequence_1.InsertCallback(1, () => {

            int enemyCount = enemySpawn.EnemyCount();
            float hpr = (userAbility.GetAbility(Ability.적수에따라HP회복_최대1000) / 1000f + 1) * enemyCount;

            HP_Recovery((int)(userAbility.GetAbility(Ability.체력) * hpr));
        }).SetLoops(-1, LoopType.Incremental);
        hprecoverySequence_1.Play();
    }
    void HPRecovery_1Stop()
    {
        if (hprecoverySequence_1 != null)
        {
            hprecoverySequence_1.Kill();
        }
    }

    // 사이즈
    public void SizeSetting(float size)
    {
        this.transform.localScale = new Vector3(size, size, size);
    }
}
