using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public List<GameObject> objs = new List<GameObject>();

    public Scrollbar bgScrollbar, fxScrollbar;
    public Dropdown languageDropdown;

    public void Open()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(true);
        }
        Setting();
    }
    public void Close()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetActive(false);
        }
    }
    void Setting()
    {
        bgScrollbar.value = SoundManager.Instance.bgm_volume;
        fxScrollbar.value = SoundManager.Instance.fx_volume;
        languageDropdown.value = (int)Language.Instance.languageType;
    }

    public void BG_OnValueChanged()
    {
        SoundManager.Instance.BGMVolumeSetting(bgScrollbar.value);
    }
    public void FX_OnValueChanged()
    {
        SoundManager.Instance.FxVolumeSetting(fxScrollbar.value);
    }
    public void LanguageOnValueChanged()
    {
        Language.Instance.ChangeLanguage((LanguageType)languageDropdown.value);
    }
}
