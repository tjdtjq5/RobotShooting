using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMove : MonoBehaviour
{
    public JoyStick joyStick;
    public UserAbility userAbility;
    public AniManager aniManager;
    public Player player;

    [Header("Clamp")]
    public BoxCollider2D bound;
    [HideInInspector] public Vector2 minBound;
    [HideInInspector] public Vector2 maxBound;
    float clampedX = 0;
    float clampedY = 0;
    float speedVelocity = 0.025f;

    [HideInInspector] public string waitAniKey = "Wait";
    [HideInInspector] public string walkL = "WalkL";
    [HideInInspector] public string walkR = "WalkR";


    private void Start()
    {
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }

    private void FixedUpdate()
    {
        if (!player.isBattle)
        {
            return;
        }

        if (joyStick.movePosition != Vector3.zero)
        {
            // Move
            this.transform.position += new Vector3(joyStick.movePosition.x * userAbility.GetAbility(Ability.이동속도) * speedVelocity * Time.fixedDeltaTime, joyStick.movePosition.y * userAbility.GetAbility(Ability.이동속도) * speedVelocity * Time.fixedDeltaTime, 0);
            clampedX = Mathf.Clamp(this.transform.position.x, minBound.x, maxBound.x);
            clampedY = Mathf.Clamp(this.transform.position.y, minBound.y, maxBound.y);
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);

            if ((joyStick.movePosition.x > 0))
            {
                aniManager.PlayAnimation(walkR, false);
            }
            else
            {
                aniManager.PlayAnimation(walkL, false);
            }
        }
        else
        {
            aniManager.PlayAnimation(waitAniKey, false);
        }
    }
}
