using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : Singleton<UserInfo>
{
    int core; const string coreKey = "core";

    public int Core { set { core = value; PlayerPrefs.SetInt(coreKey, core); CoreUI.Instance.Setting(); } get { return core; } }

    private void Awake()
    {
        Load();
    }

    private void Load()
    {
        core = PlayerPrefs.GetInt(coreKey);
    }

    [Button("코어 충전")]
    void CoreTest(int _core)
    {
        Core += _core;
    }
}
