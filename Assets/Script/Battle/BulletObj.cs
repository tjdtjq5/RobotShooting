﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BulletObj : MonoBehaviour
{
    BulletSO bulletSO;
    Transform target;
    int atk, cri, cridmg, duration;
    float movespeed = 0;
    BulletType bulletType;
    BulletHost bulletHost;

    // 유도
    float u_time = 0;
    float dis = 0;

    private void Start()
    {
        this.gameObject.tag = "Bullet";
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnEnable()
    {
        bulletSO = null;
        target = null;
    }

    // 타겟, 공격력, 내구도 
    public void Spawn(BulletSO _bulletSO, BulletType _bulletType ,Transform _target, float _angle ,BulletHost _bulletHost ,int _atk, int _cri, int _cridmg, int _duration)
    {
        this.bulletSO = _bulletSO;
        this.target = _target;
        this.bulletType = _bulletType;
        this.bulletHost = _bulletHost;
        this.atk = _atk + _bulletSO.atk + _bulletSO.atkPlus;
        this.cri = _cri;
        this.cridmg = _cridmg;
        this.duration = _duration + _bulletSO.bulletDuration;
        this.movespeed = _bulletSO.movespeed;

        this.transform.localRotation = Quaternion.Euler(0, 0, _angle);

        switch (bulletType)
        {
            case BulletType.기본:
                break;
            case BulletType.유도:
                u_time = 0;
                dis = Vector3.Distance(this.transform.position, _target.position);
                this.transform.localRotation = Quaternion.Euler(0, 0, _angle + Random.Range(-_angle, _angle));
                break;
            case BulletType.다발:
                BulletSpawn.Instance.Spawn(_bulletSO, BulletType.기본, this.transform, _target, _angle + 30, _bulletHost, _atk, _cri, _cridmg, _duration);
                BulletSpawn.Instance.Spawn(_bulletSO, BulletType.기본, this.transform, _target, _angle - 30, _bulletHost, _atk, _cri, _cridmg, _duration);
                break;
            case BulletType.관통:
                break;
            case BulletType.위성레이저:
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                this.transform.position = new Vector2(target.position.x, 12);
                break;
            case BulletType.전기_1:
                break;
            case BulletType.전기_2:
                break;
        }

        Invoke("Destroy", 10);
    }
    // 내구도 차감 
    public void DurationDeduct()
    {
        duration--;
        if (duration <= 0)
        {
            Destroy();
        }
    }
    void Destroy()
    {
        CancelInvoke();
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (bulletSO == null)
        {
            return;
        }

        switch (bulletType)
        {
            case BulletType.기본:
                this.transform.Translate(Vector2.up * movespeed * Time.fixedDeltaTime);
                break;
            case BulletType.유도:
                if (target != null)
                {
                    u_time += Time.fixedDeltaTime;
                    if (u_time < 0.3f)
                    {
                        movespeed = Time.fixedDeltaTime * bulletSO.movespeed;
                        transform.Translate(Vector2.up * movespeed);
                    }
                    else
                    {
                        // 1.5초 이후 타겟방향으로 lerp위치이동 합니다

                        movespeed += Time.fixedDeltaTime * bulletSO.movespeed;
                        float t = movespeed / dis;

                        transform.position = Vector3.LerpUnclamped(transform.position, target.position, t);

                        float angle = Function.Tool.GetAngle(this.transform.position, target.position) - 90;
                        this.transform.localRotation = Quaternion.Euler(0, 0, angle);
                    }
                }
                else
                {
                    string tag = (bulletHost == BulletHost.플레이어) ? "Player" : "Enemy";
                    Transform tempTarget = Function.Tool.SearchCharacter(20, this.transform.position, tag);
                    if (tempTarget == null)
                        Destroy();
                    target = tempTarget;
                }
           
                break; 
            case BulletType.다발:
                this.transform.Translate(Vector2.up * movespeed * Time.fixedDeltaTime);
                break;
            case BulletType.관통:
               

                this.transform.Translate(Vector2.up * movespeed * Time.fixedDeltaTime);
                break;
            case BulletType.위성레이저:
                this.transform.Translate(Vector2.down * movespeed * Time.fixedDeltaTime);
                break;
            case BulletType.전기_1:
                this.transform.Translate(Vector2.up * movespeed * Time.fixedDeltaTime);
                break;
            case BulletType.전기_2:
                this.transform.Translate(Vector2.up * movespeed * Time.fixedDeltaTime);
                break;
            default:
                this.transform.Translate(Vector2.up * movespeed * Time.fixedDeltaTime);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.GetComponent<EnemyObj>() != null)
        {
            if (bulletHost == BulletHost.플레이어)
            {
                EnemyObj collisionEnemy = collision.transform.GetComponent<EnemyObj>();
                collisionEnemy.Hit(atk, cri, cridmg);
                if (bulletType == BulletType.관통
                     || bulletType == BulletType.위성레이저
                      || bulletType == BulletType.전기_1
                       || bulletType == BulletType.전기_2)
                {

                }
                else
                {
                    Destroy();
                }
            }
        }
        if (collision.transform.GetComponent<Player>() != null)
        {
            if (bulletHost == BulletHost.적)
            {
                Player collisionPlayer = collision.transform.GetComponent<Player>();
                collisionPlayer.Hit(atk, cri, cridmg);
                if (bulletType == BulletType.관통
                    || bulletType == BulletType.위성레이저
                     || bulletType == BulletType.전기_1
                      || bulletType == BulletType.전기_2)
                {

                }
                else
                {
                    Destroy();
                }
            }
        }
        else if (collision.transform.GetComponent<BulletObj>() != null)
        {
            BulletObj collisionBullet = collision.transform.GetComponent<BulletObj>();
            if (bulletHost != collisionBullet.bulletHost)
            {
                DurationDeduct();
            }
        }
    }
}
