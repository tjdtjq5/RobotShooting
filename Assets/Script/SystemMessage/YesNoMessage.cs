using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class YesNoMessage : Singleton<YesNoMessage>
{
    public List<GameObject> objects = new List<GameObject>();
    public Text scripText;

    System.Action okCallback;

    public void Show(string _script , System.Action _okCallback)
    {
        scripText.text = _script;
        okCallback = _okCallback;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(true);
        }
    }

    public void OnClickOk()
    {
        if (okCallback != null)
        {
            okCallback();
        }
    }

    public void OnClickExit()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
