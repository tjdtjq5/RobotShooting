using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : Singleton<Player>
{
    public SpriteRenderer characterSprite, weaponSprite;
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
    [HideInInspector] public int hp = 0;

    [Title("오브젝트")]
    public ParticleSystem restartEffect;

    // 특수능력 bool 
    [HideInInspector] public bool isElectric;
    [HideInInspector] public int electricCount = 1;
    [HideInInspector] public bool isCollection;
    [HideInInspector] public bool isReflect;
    [HideInInspector] public bool isLaser; float r_time = 0; public BulletSO laserSO;
    [HideInInspector] public bool isGyroscope;
    [HideInInspector] public bool isPackman;
    [HideInInspector] public bool isWiper; bool isRW;
    [HideInInspector] public bool isComposure;
    [HideInInspector] public bool isElecitricField;
    [HideInInspector] public bool isResurrection;
    [HideInInspector] public bool isStarlink;
    [HideInInspector] public bool isHighnon;
    [HideInInspector] public bool isElectro;
    [HideInInspector] public bool isMultiGubter;
    [HideInInspector] public bool isReflecterBarrior;
    [HideInInspector] public bool isAutoCreate;
    [HideInInspector] public bool isSpo;

    // 무적
    bool isInvince;
    bool isBarrior;


    private void Start()
    {
        this.gameObject.tag = "Player";
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void BattleSetting()
    {
        isBattle = true;
        isElectric = false;
        isCollection = false;
        isReflect = false;
        isLaser = false;
        isGyroscope = false;
        isWiper = false;
        isComposure = false;
        isElecitricField = false;
        isResurrection = true;
        isStarlink = false;
        isHighnon = false;
        isElectro = false;
        isMultiGubter = false;
        isReflecterBarrior = false;
        isAutoCreate = false;
        isSpo = false;

        userAbility.BattleSetting();

        hp = userAbility.GetAbility(Ability.체력);

        HP_Setting();

        HPRecovery_1Start();
    }

    public void Hit(int dmg, int cri, int cridmg, EnemyObj _enemyObj)
    {
        if (!isBattle)
            return;

        bool isMiss = Function.GameInfo.IsCritical(userAbility.GetAbility(Ability.회피_최대1000));
        if (isMiss)
        {
            // 회피
            DmgSpawn.Instance.MissSpawn(this.transform.position);
            return;
        }

        if (isInvince)
        {
            if (isComposure)
            {
                HP_Recovery(1);
            }

            // 무적
            return;
        }

        if (isBarrior)
        {
            // 방어막
            isBarrior = false;

            if (isReflecterBarrior)
            {
                Electric.Instance.ElectricSpawn_1(this.transform, 16, 60, 0, 0);
            }

            return;
        }

        int shild = userAbility.GetAbility(Ability.방어력);
        shild = (int)(shild * (userAbility.GetAbility(Ability.멈춰있을때방어력증가) / 1000f * (int)userMove.idleTime) + shild);

        if (isReflect && _enemyObj!= null && _enemyObj.gameObject.activeSelf)
        {
            int reflact = shild + (shild * (shild + 10 / 100));
            _enemyObj.Hit(reflact,0,0, null, 0);
        }

        Invince(userAbility.GetAbility(Ability.피해를입은후무적) / 1000f);

        bool isCri = Function.GameInfo.IsCritical(cri);
        dmg = isCri ? (int)(dmg * (cridmg / 1000f)) : dmg;

        dmg -= shild;
        if (dmg < 1)
        {
            dmg = 1;
        }

        Vector2 pos = this.transform.position;
        pos = new Vector2(pos.x, pos.y + 1.5f);
        DmgSpawn.Instance.Spawn(pos, dmg.ToString(), isCri , false);

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

        if (hp >= userAbility.GetAbility(Ability.체력))
        {
            hp = userAbility.GetAbility(Ability.체력);
        }

        HP_Setting();
    }

    void Death()
    {
        if (isResurrection)
        {
            isResurrection = false;
            hp = userAbility.GetAbility(Ability.체력);
            HP_Setting();
            for (int i = 0; i < enemySpawn.spawnObjList.Count; i++)
            {
                if (enemySpawn.spawnObjList[i].activeSelf)
                {
                    enemySpawn.spawnObjList[i].GetComponent<EnemyObj>().Destroy();
                }
            }

            restartEffect.Play();

            return;
        }

        battleManager.BattleFailure();
    }

    public void BattleEnd(System.Action _callback)
    {
        isBattle = false;
        joyStick.DragEnd();
        HPRecovery_1Stop();
        SizeSetting(1);
        Barrior_Stop();
        Wipper_Stop();

        weaponTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.6f);

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

        if (MultipleShot.Instance.isOpenItem)
        {
            for (int i = -3; i < 3; i++)
            {
                float angle = weaponTransform.localEulerAngles.z + (15 * i);
                atk += MultipleShot.Instance.value;
                BulletSpawn.Instance.Spawn(this.transform, _bulletSO, _bulletSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어,
                    (int)atk,
                    userAbility.GetAbility(Ability.치명데미지_최대1000)
                    , userAbility.GetAbility(Ability.치명데미지_최대1000)
                    , userAbility.GetAbility(Ability.손상피해)
                    , userAbility.GetAbility(Ability.발사체내구도)
                    , userAbility.GetAbility(Ability.발사체크기));
            }
        }
        else if (MultipleShot.Instance.isMultipleShot)
        {
            for (int i = -1; i < 2; i++)
            {
                float angle = weaponTransform.localEulerAngles.z + (45 * i);
                atk += MultipleShot.Instance.value;
                BulletSpawn.Instance.Spawn(this.transform, _bulletSO, _bulletSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어,
                    (int)atk,
                    userAbility.GetAbility(Ability.치명데미지_최대1000)
                    , userAbility.GetAbility(Ability.치명데미지_최대1000)
                    , userAbility.GetAbility(Ability.손상피해)
                    , userAbility.GetAbility(Ability.발사체내구도)
                    , userAbility.GetAbility(Ability.발사체크기));
            }
        }
        else
        {
            float angle = weaponTransform.localEulerAngles.z;
            BulletSpawn.Instance.Spawn(this.transform, _bulletSO, _bulletSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어,
                (int)atk,
                userAbility.GetAbility(Ability.치명데미지_최대1000)
                , userAbility.GetAbility(Ability.치명데미지_최대1000)
                , userAbility.GetAbility(Ability.손상피해)
                , userAbility.GetAbility(Ability.발사체내구도)
                , userAbility.GetAbility(Ability.발사체크기));
        }
    }

    void LaserAttack(Transform _enemy)
    {
        r_time = 0;
        float angle = Function.Tool.GetAngle(this.transform.position, _enemy.position) - 90;
        BulletSpawn.Instance.Spawn(this.transform, laserSO, laserSO.bulletType, bulletPos, _enemy, angle, BulletHost.플레이어, 0, 0, 0, 0, 0, 1);
    }

    private void FixedUpdate()
    {
        if (!isBattle)
            return;

        a_time += Time.fixedDeltaTime;
        r_time += Time.fixedDeltaTime;

        Transform enemy = Function.Tool.SearchCharacter(searchRadius, this.transform.position, "Enemy");
        if (!isWiper)
        {
            if (enemy != null)
            {
                float angle = Function.Tool.GetAngle(bulletPos.position, enemy.position) - 90;
                weaponTransform.DOLocalRotate(new Vector3(0, 0, angle), 0.6f);
            }
            else
            {
                weaponTransform.DOLocalRotate(new Vector3(0, 0, 0), 0.6f);
            }
        }

        if (enemy != null)
        {
            BulletSO bulletSO = userWeapon.GetEqipWeapon().bulletSO;
            float atkspeed = bulletSO.atkspeed / (userAbility.GetAbility(Ability.공격속도) / 1000f + 1);
            float atkspeedPercent = (int)userMove.idleTime * userAbility.GetAbility(Ability.멈춰있을때공격속도증가_최대1000) * 0.25f;
            atkspeed = atkspeed - (atkspeed * atkspeedPercent / 1000);
            if (atkspeed < 0.1f)
            {
                atkspeed = 0.1f;
            }
            if (a_time >= atkspeed)
            {
                Attack(bulletSO, enemy);
            }

            if (isLaser && r_time >= laserSO.atkspeed)
            {
                LaserAttack(enemy);
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
        characterSprite.color = Color.gray;
        weaponSprite.color = Color.gray;

        invinceSequence.InsertCallback(time, () => { isInvince = false;
            characterSprite.color = Color.white;
            weaponSprite.color = Color.white;
        });
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
            float hpr = (userAbility.GetAbility(Ability.적수에따라HP회복_최대1000)) * enemyCount;

            HP_Recovery((int)hpr);
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

    public void DoubleSize()
    {
        this.transform.localScale *= 2;
    }

    // 방어막
    Sequence barriorSequence;
    public void Barrior_Start(float time)
    {
        if (barriorSequence != null)
        {
            barriorSequence.Kill();
        }

        barriorSequence = DOTween.Sequence();

        barriorSequence.InsertCallback(time, () => {

            isBarrior = true;
            
        }).SetLoops(-1, LoopType.Incremental);
        barriorSequence.Play();
    }
    void Barrior_Stop()
    {
        if (barriorSequence != null)
        {
            barriorSequence.Kill();
        }
    }
    // 와이퍼
    Sequence wipperSequence;
    [ContextMenu("Wipper_Start")]
    public void Wipper_Start()
    {
        if (wipperSequence != null)
        {
            wipperSequence.Kill();
        }

        isWiper = true;

        wipperSequence = DOTween.Sequence();

        wipperSequence.InsertCallback(1, () => {
            if (isRW)
            {
                weaponTransform.DOLocalRotate(new Vector3(0, 0, 45), 1.2f).OnComplete(()=> isRW = !isRW);
            }
            else
            {
                weaponTransform.DOLocalRotate(new Vector3(0, 0, -45), 1.2f).OnComplete(() => isRW = !isRW);
            }
        }).SetLoops(-1, LoopType.Incremental);
        wipperSequence.Play();
    }
    void Wipper_Stop()
    {
        if (wipperSequence != null)
        {
            wipperSequence.Kill();
        }
        isWiper = false;
    }
}
