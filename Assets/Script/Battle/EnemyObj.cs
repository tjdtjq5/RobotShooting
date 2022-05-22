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

    float a_time = 0;

    private void Start()
    {
        this.gameObject.tag = "Enemy";
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void OnEnable()
    {
        isSpawn = false;
    }

    public void Spawn(EnemySO _enemySO, Transform _target, Vector2 _startPos, float _endPosY)
    {
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

    public void Hit(int dmg , int cri , int cridmg)
    {
        hp -= dmg;

        DmgSpawn.Instance.Spawn(this.transform.position, dmg.ToString());

        spriteTrans.DOKill();
        spriteTrans.localScale = Vector3.one;
        spriteTrans.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.1f)
                   .OnComplete(() => {
                       spriteTrans.DOScale(Vector3.one, 0.1f);
                   });

        spriteRenderer.DOKill();
        spriteRenderer.color = Color.white;
        spriteRenderer.DOColor(new Color(1, 194f / 255f, 194f / 255f, 1), 0.1f)
                      .OnComplete(() => {
                          spriteRenderer.DOColor(Color.white, 0.1f);
                      });

        HitSpawn.Instance.Spawn(this.transform.position);

        if (hp <= 0)
        {
            Destroy();
        }
    }

    public void Attack(BulletSO _bulletSO, Transform _enemy)
    {
        a_time = 0;
        float angle = Function.Tool.GetAngle(this.transform.position, _enemy.position) - 90;
        BulletSpawn.Instance.Spawn(_bulletSO, _bulletSO.bulletType, bulletTrans, _enemy, angle, enemySO.bulletHost , 10, 10, 10, 1);
    }

    void Destroy()
    {
        BreakSpawn.Instance.Spawn(this.transform.position);
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
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
    }
}
