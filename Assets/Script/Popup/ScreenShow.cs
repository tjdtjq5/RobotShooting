using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShow : Singleton<ScreenShow>
{
    public List<ScreenShowInfo> screenShowInfos = new List<ScreenShowInfo>();
    public Image pannelImg;

    List<Sprite> sprites;
    System.Action callback;
    int index = 0;

    public void Show(string _code , System.Action _callback)
    {
        pannelImg.gameObject.SetActive(true);
        sprites = screenShowInfos.Find(n => n.code == _code).spriteList;
        callback = _callback;
        index = 0;
        pannelImg.sprite = sprites[index];
    }

    public void OnClickNext()
    {
        if (index >= sprites.Count - 1)
        {
            pannelImg.gameObject.SetActive(false);
            callback();
            return;
        }

        index++;
        pannelImg.sprite = sprites[index];
    }
}
[System.Serializable]
public class ScreenShowInfo
{
    public string code;
    public List<Sprite> spriteList = new List<Sprite>();
}