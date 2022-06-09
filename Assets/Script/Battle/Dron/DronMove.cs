using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronMove : MonoBehaviour
{

    [Header("Clamp")]
    BoxCollider2D bound;
    [HideInInspector] public Vector2 minBound;
    [HideInInspector] public Vector2 maxBound;
    float clampedX = 0;
    float clampedY = 0;
    float speedVelocity = 0.025f;
    int speed = 100;

    private void Start()
    {
        bound = EnemySpawn.Instance.bounds[EnemySpawn.Instance.bounds.Count - 1];
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        TargetPosSetting();
    }

    Vector2 targetPos;

    void TargetPosSetting()
    {
        targetPos = new Vector2(Random.Range(minBound.x, maxBound.x), Random.Range(minBound.y, maxBound.y));
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed * speedVelocity * Time.fixedDeltaTime);
        if (Vector2.Distance(this.transform.position , targetPos) <= 0.1f)
        {
            TargetPosSetting();
        }
    }
}
