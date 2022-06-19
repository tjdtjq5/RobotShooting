using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public List<GameObject> objs = new List<GameObject>();
    const string upgradeKey = "upgrade";
    public UpgradeSO upgradeSO;

    public Text restartCoreText, upgardeCoreText;

    public Transform listTransform;
    public GameObject upgradeCard;
    public GameObject upgardeMaxCard;

    int reStartCore = 2;
    int upgradeCore = 1000;

    public AudioSource failPerchaseAudio;

    private void Start()
    {
        restartCoreText.text = reStartCore.ToString();
        upgardeCoreText.text = upgradeCore.ToString();
    }

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
        if (listTransform.childCount == 0)
        {
            for (int i = 0; i < upgradeSO.upgradeDatas.Count; i++)
            {
                Instantiate(upgradeCard, listTransform);
            }
        }

        for (int i = 0; i < upgradeSO.upgradeDatas.Count; i++)
        {
            Transform card = listTransform.GetChild(i);
            UpgradeData upgradeData = upgradeSO.upgradeDatas[i];

            Image iconImg = card.Find("icon").GetComponent<Image>();
            iconImg.sprite = upgradeData.icon;

            Text nameText = card.Find("name").GetComponent<Text>();
            nameText.text = Language.Instance.GetScript(upgradeData.nameTextCode);
            nameText.font = Language.Instance.GetFont();

            Transform countList = card.Find("list");
            if (countList.childCount == 0)
            {
                for (int j = 0; j < upgradeData.maxCount; j++)
                {
                    Instantiate(upgardeMaxCard, countList);
                }
            }

            int upgradeCount = GetUpgradeCount(upgradeData.code);
            for (int j = 0; j < countList.childCount; j++)
            {
                countList.GetChild(j).GetComponent<Image>().color = (j < upgradeCount) ? new Color(122 / 255f, 255 / 255f, 254 / 255f, 1) : Color.white;
            }
        }
    }
    public void OnClickRestart()
    {
        if (UserInfo.Instance.Core < reStartCore)
        {
            Debug.Log("코어 부족");
            return;
        }

        int temp = 0;
        for (int i = 0; i < upgradeSO.upgradeDatas.Count; i++)
        {
            UpgradeData upgradeData = upgradeSO.upgradeDatas[i];
            temp += GetUpgradeCount(upgradeData.code);
        }
        if (temp <= 0)
        {
            return;
        }

        UserInfo.Instance.Core -= reStartCore;

        for (int i = 0; i < upgradeSO.upgradeDatas.Count; i++)
        {
            UpgradeData upgradeData = upgradeSO.upgradeDatas[i];
            int upgradeCount = GetUpgradeCount(upgradeData.code);

            UserInfo.Instance.Core += upgradeCore * upgradeCount;

            PlayerPrefs.SetInt(upgradeKey + upgradeData.code, 0);
        }


        Setting();
    }
    public void OnClickUpgrade()
    {
        List<UpgradeData> dataList = new List<UpgradeData>();
        for (int i = 0; i < upgradeSO.upgradeDatas.Count; i++)
        {
            UpgradeData upgradeData = upgradeSO.upgradeDatas[i];
            int upgradeCount = GetUpgradeCount(upgradeData.code);
            if (upgradeData.maxCount > upgradeCount)
            {
                dataList.Add(upgradeData);
            }
        }

        if (dataList.Count == 0)
        {
            Debug.Log("더 이상 업그레이드 할 수 있는것이 없음");
            SoundManager.Instance.FXSoundPlay(failPerchaseAudio);
            return;
        }

        if (UserInfo.Instance.Core < upgradeCore)
        {
            Debug.Log("코어 부족");
            SoundManager.Instance.FXSoundPlay(failPerchaseAudio);
            return;
        }

        UserInfo.Instance.Core -= upgradeCore;
     

        UpgradeData upgrade = dataList[Random.Range(0, dataList.Count)];
        int upCount = GetUpgradeCount(upgrade.code) + 1;
        PlayerPrefs.SetInt(upgradeKey + upgrade.code, upCount);

        UpgradeMessage.Instance.Show(Language.Instance.GetScript(upgrade.nameTextCode) + " 증가");

        Setting();
    }

    public int GetUpgradeCount(string _code)
    {
        return PlayerPrefs.HasKey(upgradeKey + _code) ? PlayerPrefs.GetInt(upgradeKey + _code) : 0;
    }
}
