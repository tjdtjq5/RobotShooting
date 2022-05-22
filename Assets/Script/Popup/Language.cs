using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : Singleton<Language>
{
    public TextSO textSO;
    [HideInInspector] public LanguageType languageType;
    const string key = "language";

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
        PlayerPrefs.SetInt(key, (int)_languageType);
    }
    public string GetScript(string _code)
    {
        TextData textData = textSO.textDatas.Find(n => n.code == _code);
        if (textData == null)
        {
            Debug.LogError("데이터가 없습니다 코드 : " + _code);
            return "데이터가 없습니다";
        }

        switch (languageType)
        {
            case LanguageType.한국어:
                return textData.ko;
            case LanguageType.영어:
                return textData.en;
            case LanguageType.일본어:
                return textData.jp;
            case LanguageType.중국어:
                return textData.cn;
            default:
                return textData.en;
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
