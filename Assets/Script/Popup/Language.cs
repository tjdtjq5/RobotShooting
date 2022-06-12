using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : Singleton<Language>
{
    public TL TL;
    [HideInInspector] public LanguageType languageType;
    const string key = "language";

    public Font koFont, enFont, jpFont, chFont;

    private void Awake()
    {
        if (PlayerPrefs.HasKey(key))
        {
            languageType = (LanguageType)PlayerPrefs.GetInt(key);
        }
        else
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Korean:
                    languageType = LanguageType.한국어;
                    break;
                case SystemLanguage.Japanese:
                    languageType = LanguageType.일본어;
                    break;
                case SystemLanguage.Chinese:
                    languageType = LanguageType.중국어;
                    break;
                case SystemLanguage.English:
                    languageType = LanguageType.영어;
                    break;
                default:
                    languageType = LanguageType.한국어;
                    break;
            }
        }
    }
    public void ChangeLanguage(LanguageType _languageType)
    {
        languageType = _languageType;
        PlayerPrefs.SetInt(key, (int)_languageType);
    }
    public string GetScript(string _code)
    {
        TLData textData = null;
        for (int i = 0; i < TL.dataArray.Length; i++)
        {
            if (TL.dataArray[i].Code == _code)
            {
                textData = TL.dataArray[i];
            }
        }

        if (textData == null)
        {
            Debug.LogError("데이터가 없습니다 코드 : " + _code);
            return "데이터가 없습니다";
        }

        switch (languageType)
        {
            case LanguageType.한국어:
                return textData.Ko.Replace('/', '\n');
            case LanguageType.영어:
                return textData.En.Replace('/', '\n');
            case LanguageType.일본어:
                return textData.Jp.Replace('/', '\n');
            case LanguageType.중국어:
                return textData.Ch.Replace('/', '\n');
            default:
                return textData.En.Replace('/', '\n');
        }
    }
    public Font GetFont()
    {
        switch (languageType)
        {
            case LanguageType.한국어:
                return koFont;
            case LanguageType.영어:
                return enFont;
            case LanguageType.일본어:
                return jpFont;
            case LanguageType.중국어:
                return chFont;
            default:
                return koFont;
        }
    }
}
public enum LanguageType
{
    한국어,
    영어,
    일본어,
    중국어
}
