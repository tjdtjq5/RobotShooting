using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdShot : Singleton<ColdShot>
{
    [HideInInspector] public bool isCold = false;
    public int DefaultValue;
    [HideInInspector] public int value = 1000;
    public float openItemTime;
    public ParticleSystem particleSystem;

    public void Shot()
    {
        value = DefaultValue;
        isCold = true;
    }

    public void Upgrade(int _value)
    {
        value += _value;
    }

    public void End()
    {
        isCold = false;
        if (openItemCoroutine != null)
        {
            StopCoroutine(openItemCoroutine);
        }
    }

    public void OpenItem()
    {
        if (openItemCoroutine != null)
        {
            StopCoroutine(openItemCoroutine);
        }
        openItemCoroutine = OpenItemCoroutine();
        StartCoroutine(openItemCoroutine);
    }

    IEnumerator openItemCoroutine;

    IEnumerator OpenItemCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(openItemTime);
        while (true)
        {
            yield return waitTime;
            BulletSpawn.Instance.AllDestroy();
            particleSystem.Play();
        }
    }
}
