using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLanguage : Singleton<TextLanguage>
{
    public List<TextLanguageInfo> textLanguageInfos = new List<TextLanguageInfo>();

    private void Start()
    {
        Setting();
    }

    public void Setting()
    {
        for (int i = 0; i < textLanguageInfos.Count; i++)
        {
            string code = textLanguageInfos[i].code;
            textLanguageInfos[i].text.text = Language.Instance.GetScript(code);
            textLanguageInfos[i].text.font = Language.Instance.GetFont();
        }
    }
}

[System.Serializable]
public class TextLanguageInfo
{
    public string code;
    public Text text;
}