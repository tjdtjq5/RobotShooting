using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleShot : Singleton<MultipleShot>
{
    [HideInInspector] public bool isMultipleShot = false;
    [HideInInspector] public bool isOpenItem = false;
    [HideInInspector] public int value;

    public void Shot()
    {
        value = 0;
        isMultipleShot = true;
    }
    public void End()
    {
        isMultipleShot = false;
        isOpenItem = false;
    }
    public void Upgrade(int _value)
    {
        value += _value;
    }
    public void OpenItem()
    {
        isOpenItem = true;
    }
}
