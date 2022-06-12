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

    System.Action okCallback, noCallback;

    public void Show(string _script , System.Action _okCallback, System.Action _noCallback = null)
    {
        scripText.text = _script;
        scripText.font = Language.Instance.GetFont();
        okCallback = _okCallback;
        noCallback = _noCallback;

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
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
    }

    public void OnClickExit()
    {
        if (noCallback != null)
        {
            noCallback();
        }
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
