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

    public void Spawn(Vector2 _pos, string _dmgScript, bool isCri , bool isSupper)
    {
        if (isSupper)
        {
            text.color = new Color(1, 54 / 255f, 0, 1);
            text.fontSize = 65;
        }
        else if (isCri)
        {
            text.color = new Color(1, 173 / 255f, 0,1);
            text.fontSize = 55;
        }
        else
        {
            text.color = Color.white;
            text.fontSize = 45;
        }

        text.text = _dmgScript;
        this.transform.position = _pos;
        this.transform.DOJump(new Vector2(_pos.x + Random.Range(-maxRX, maxRX), _pos.y + maxY) , 1 , 1 , jumpTime);
        Invoke("Destroy", desTime);
    }

    public void MissSpawn(Vector2 _pos)
    {
        text.color = new Color(0, 159 / 255f, 1, 1);
        text.text = "Miss";
        this.transform.position = _pos;
        this.transform.DOJump(new Vector2(_pos.x + Random.Range(-maxRX, maxRX), _pos.y + maxY), 1, 1, jumpTime);
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
