using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DmgObj : MonoBehaviour
{
    public Text text;

    float maxRX = 0.3f;
    float maxY = 0.75f;

    float jumpTime = 0.35f;
    float desTime = 1.5f;

    public void Spawn(Vector2 _pos, string _dmgScript)
    {
        text.text = _dmgScript;
        this.transform.position = _pos;
        this.transform.DOJump(new Vector2(_pos.x + Random.Range(-maxRX, maxRX), _pos.y + maxY) , 1 , 1 , jumpTime);
        Invoke("Destroy", desTime);
    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CancelInvoke();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
