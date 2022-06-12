using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyObj : MonoBehaviour
{
    public Transform spriteTrans;
    public SpriteRenderer spriteRenderer;
    public Transform bulletTrans;

    EnemySO enemySO;

    Transform target;
    bool isSpawn = false;
    float movespeedVelocity = 100;
    float movespeed = 0;

    int hp = 0;
    int tick = 0;

    float a_time = 10;
    float e_time = 0;

    private void Start()
    {
        this.gameObject.tag = "Enemy";
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnEnable()
    {
        a_time = 10;
        e_time = 0;
        tick = 0;
        isSpawn = false;
    }

    public void Spawn(EnemySO _enemySO, Transform _target, Vector2 _startPos, float _endPosY)
    {
        spriteRenderer.color = Color.white;
        this.enemySO = _enemySO;
        this.target = _target; 
        this.transform.position = _startPos;
        this.GetComponent<BoxCollider2D>().size = enemySO.colliderSize;
        spriteRenderer.sprite = enemySO.sprite;

        this.transform.DOMoveY(_endPosY, enemySO.spawnTime).OnComplete(() => {
            isSpawn = true;
        });

        hp = _enemySO.hp;
        movespeed = movespeedVelocity / _enemySO.movespeed;
    }

    public void Hit(int dmg , int cri , int cridmg, BulletSO _bulletSO , int ticDmg)
    {
        if (!isSpawn)
        {
            return;
        }

        bool isCri = (Player.Instance.isGyroscope && hp == enemySO.hp) ? true : Function.GameInfo.IsCritical(cri);
        bool supperCri = false;

        int t_dmg = isCri ? (int)(dmg * (cridmg / 1000f)) + dmg : dmg;

        if (Player.Instance.isHighnon)
        {
            supperCri = Function.GameInfo.IsCritical(cri);
            t_dmg = supperCri ? (int)(t_dmg * (cridmg / 1000f)) + t_dmg : t_dmg;
        }

        t_dmg -= UserAbility.Instance.GetAbility(Ability.방어력);
        if (t_dmg <= 0)
        {
            t_dmg = 1;
        }

        if (_bulletSO != null)
        {
            if (_bulletSO.bulletType != BulletType.위성레이저)
            {
                int hpr = (int)(t_dmg * (UserAbility.Instance.GetAbility(Ability.HP흡수_최대1000) / 1000f));
                Player.Instance.HP_Recovery(hpr);
            }
        }

        hp -= t_dmg;

        DmgSpawn.Instance.Spawn(this.transform.position, t_dmg.ToString(), isCri , supperCri);

        HitEffect();

        HitSpawn.Instance.Spawn(this.transform.position);

        TickStart(ticDmg);

        if (hp <= 0)
        {
            Destroy();

            if (Player.Instance.isElectric)
            {
                Electric.Instance.ElectricSpawn_1(this.transform, 8, (int)(dmg * 0.7f), 0, 0);
                return;
            }

            if (_bulletSO != null && _bulletSO.bulletType == BulletType.전기_1)
            {
                float p = Player.Instance.isElectro ? 0.7f : 0.3f;
                Electric.Instance.ElectricSpawn_2(this.transform, 2, (int)(dmg * p), 0, 0);
                return;
            }
        }
    }

    void HitEffect()
    {
        spriteTrans.DOKill();
        spriteTrans.localScale = Vector3.one;
        spriteTrans.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f)
                   .OnComplete(() => {
                       spriteTrans.DOScale(Vector3.one, 0.1f);
                   });

        spriteRenderer.DOKill();
        Color spriteColor = ((float)hp / enemySO.hp > 0.1f) ? Color.white : Color.gray;
        spriteRenderer.color = spriteColor;
        spriteRenderer.DOColor(new Color(1, 194f / 255f, 194f / 255f, 1), 0.1f)
                      .OnComplete(() => {
                          spriteRenderer.DOColor(spriteColor, 0.1f);
                      });
    }

    public void Attack(BulletSO _bulletSO, Transform _enemy)
    {
        a_time = 0;
        float angle = Function.Tool.GetAngle(this.transform.position, _enemy.position) - 90;
        int atk = enemySO.atk + _bulletSO.atk;
        BulletSpawn.Instance.Spawn(this.transform, _bulletSO, _bulletSO.bulletType, bulletTrans, _enemy, angle, enemySO.bulletHost , atk, 10, 10, 0,1, 1);
    }

    public void Destroy()
    {
        BreakSpawn.Instance.Spawn(this.transform.position);

        int dorpAbility = UserAbility.Instance.GetAbility(Ability.코어드랍률_최대1000);
        if (Player.Instance.isCollection)
        {
            dorpAbility *= 2;
            UserAbility.Instance.BuffAbility(new AbilityData(Ability.체력, 1));
            Player.Instance.HP_Setting();
        }
        bool isDrop = Function.GameInfo.IsCritical(dorpAbility);
        if (isDrop)
        {
            int dropCoin = BattleManager.Instance.c_battleSO.rewardCoreCount;
            UserInfo.Instance.Core += dropCoin;
            CoreSpawn.Instance.Spawn(this.transform.position, dropCoin);
        }

        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        isSpawn = false;
        TickStop();
        spriteTrans.DOKill();
        spriteRenderer.DOKill();
        BattleManager.Instance.WaveCheck();
    }

    private void FixedUpdate()
    {
        if (!isSpawn)
        {
            return;
        }

        a_time += Time.fixedDeltaTime;


        switch (enemySO.enemyMovePattern)
        {
            case EnemyMovePattern.없음:
                break;
            case EnemyMovePattern.반대방향:
                this.transform.DOMoveX(-target.position.x, movespeed);
                break;
            case EnemyMovePattern.정방향:
                this.transform.DOMoveX(target.position.x, movespeed);
                break;
        }

        float angle = Function.Tool.GetAngle(this.transform.position, target.position) + 90;
        this.transform.DOLocalRotate(new Vector3(0, 0, angle), 0.6f);
        if (a_time >= enemySO.atkspeed)
        {
            Attack(enemySO.bulletSO, target);
        }

        if (Player.Instance.isElecitricField)
        {
            e_time += Time.fixedDeltaTime;
            if (e_time > 1)
            {
                e_time = 0;

                hp -= (int)(enemySO.hp / 100f);

                if (hp <= 0)
                {
                    Destroy();
                }
                else
                {
                    HitEffect();
                }
            }
        }
    }

    // 손상데미지 
    Sequence tickSequence;
    void TickStart(int _tick)
    {
        if (_tick <= 0)
        {
            return;
        }

        tick += _tick;

        if (tickSequence != null)
        {
            tickSequence.Kill();
        }

        float time = Player.Instance.isSpo ? 0.5f : 1;

        tickSequence = DOTween.Sequence();
        tickSequence.InsertCallback(time, () =>
        {
            hp -= tick;
            DmgSpawn.Instance.Spawn(this.transform, tick.ToString(), false, false);

            if (hp <= 0)
            {
                Destroy();
            }

        }).SetLoops(-1, LoopType.Incremental).Play();
    }
    void TickStop()
    {
        if (tickSequence != null)
        {
            tickSequence.Kill();
        }
    }
}
