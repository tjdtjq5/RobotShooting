using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObj : MonoBehaviour
{
    float desTime = 1.5f;

    public void Spawn(Vector2 _pos)
    {
        this.transform.position = _pos;
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
