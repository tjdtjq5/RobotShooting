using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyStartUI : Singleton<LobbyStartUI>
{
    public BattleManager battleManager;
    int index = 0;

    public Text difiText;

    private void Start()
    {
        Setting(GetMaxBattleSOIndex());
    }

    public void Setting(int _index)
    {
        index = _index;
        difiText.text = Language.Instance.GetScript(battleManager.battleSOs[_index].nameCode);
        difiText.font = Language.Instance.GetFont();
    }
    public void Setting()
    {
        difiText.text = Language.Instance.GetScript(battleManager.battleSOs[index].nameCode);
        difiText.font = Language.Instance.GetFont();
    }

    public void OnClickStart()
    {
        battleManager.BattleStart(battleManager.battleSOs[index]);
    }

    public void Rirght()
    {
        index++;
        if (index > GetMaxBattleSOIndex())
        {
            index = 0;
        }
        Setting(index);
    }
    public void Left()
    {
        index--;
        if (index < 0)
        {
            index = GetMaxBattleSOIndex();
        }
        Setting(index);
    }

    public int GetMaxBattleSOIndex()
    {
        int count = 0;
        for (int i = 1; i < battleManager.battleSOs.Count; i++)
        {
            string code = battleManager.battleSOs[i].code;
            if (PlayerPrefs.HasKey(battleManager.battleKey + code))
            {
                count++;
            }
        }

        return count;
    }
}
