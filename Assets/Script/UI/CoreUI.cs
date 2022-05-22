using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreUI : Singleton<CoreUI>
{
    public Text coreCountText;

    private void Start()
    {
        Setting();
    }
    public void Setting()
    {
        coreCountText.text = UserInfo.Instance.Core.ToString();
    }
}
