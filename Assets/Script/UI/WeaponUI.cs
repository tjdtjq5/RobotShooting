using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : Singleton<WeaponUI>
{
    public UserWeapon userWeapon;

    public Button bgBtn;
    public Transform list;
    public GameObject card;

    public Text nameText;

    int index = 0;

    string weaponUIIndexKey = "weaponUIIndexKey";

    public AudioSource weaponPurchaseAudio;


    private void Start()
    {
        if (PlayerPrefs.HasKey(weaponUIIndexKey))
        {
            index = PlayerPrefs.GetInt(weaponUIIndexKey);
        }

        for (int i = 0; i < userWeapon.weaponSOList.Count; i++)
        {
            Instantiate(card, list);
        }
        CardDestroy();
    }

    public void CardDestroy()
    {
        for (int i = 0; i < list.childCount; i++)
        {
            list.GetChild(i).gameObject.SetActive(false);
        }
    }


    public void Setting()
    {
        PlayerPrefs.SetInt(weaponUIIndexKey, index);

        List<WeaponSO> weaponList = userWeapon.weaponSOList;

        for (int i = 0; i < weaponList.Count; i++)
        {
            list.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = 0; i < list.childCount; i++)
        {
            Transform c = list.GetChild(i);
            c.GetComponent<Image>().color = (i == index) ? new Color(122 / 255f, 255 / 255f, 254 / 255f, 1) : Color.white;
        }

        WeaponSO selectWeaponSO = weaponList[index];
        nameText.text = Language.Instance.GetScript(selectWeaponSO.nameCode);
        nameText.font = Language.Instance.GetFont();
        bgBtn.onClick.RemoveAllListeners();

        if (userWeapon.GetHaveList().Find(n => n.code == selectWeaponSO.code) != null)
        {
            userWeapon.ChangeEqipWeapon(selectWeaponSO.code);
            bgBtn.image.color = Color.white;
        }
        else
        {
            bgBtn.image.color = new Color(0.5f, 0.5f, 0.5f, 1);
            bgBtn.onClick.AddListener(() => {
                YesNoMessage.Instance.Show(Language.Instance.GetScript(selectWeaponSO.nameCode) + "을 구매 하시겠습니까?\n" +
                    "필요한 코어 : " + selectWeaponSO.coreCount
                    , () => {
                        if (UserInfo.Instance.Core >= selectWeaponSO.coreCount)
                        {
                            UserInfo.Instance.Core -= selectWeaponSO.coreCount;
                            userWeapon.PushWeapon(selectWeaponSO.code);
                            TextMessage.Instance.Show("구매완료");
                            Setting();
                            SoundManager.Instance.FXSoundPlay(weaponPurchaseAudio);
                        }
                        else
                        {
                            TextMessage.Instance.Show("보유한 코어가 모자랍니다");
                        }
                    });;
            });
        }
    }

    public void OnClickRight()
    {
        index++;
        if (userWeapon.weaponSOList.Count <= index)
            index = 0;


        Setting();
    }

    public void OnClickLeft()
    {
        index--;
        if (index < 0)
            index = userWeapon.weaponSOList.Count - 1;


        Setting();
    }
}
