using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    public UserWeapon userWeapon;

    public Transform list;
    public GameObject card;

    public Text nameText;

    private void Start()
    {
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
        List<WeaponSO> haveList = userWeapon.GetHaveList();

        for (int i = 0; i < haveList.Count; i++)
        {
            list.GetChild(i).gameObject.SetActive(true);
        }

        WeaponSO eqipWeapon = userWeapon.GetEqipWeapon();
        int index = haveList.FindIndex(n => n.code == eqipWeapon.code);

        if (index < 0)
        {
            Debug.LogError("정보가 잘 못 되었음");
            return;
        }

        for (int i = 0; i < list.childCount; i++)
        {
            Transform c = list.GetChild(i);
            c.GetComponent<Image>().color = (i == index) ? new Color(122 / 255f, 255 / 255f, 254 / 255f, 1) : Color.white;
        }

        nameText.text = Language.Instance.GetScript(eqipWeapon.nameCode);
    }

    public void OnClickRight()
    {
        List<WeaponSO> haveList = userWeapon.GetHaveList();
        WeaponSO eqipWeapon = userWeapon.GetEqipWeapon();
        int index = haveList.FindIndex(n => n.code == eqipWeapon.code);

        index++;
        if (haveList.Count <= index)
            index = 0;

        userWeapon.ChangeEqipWeapon(index);

        Setting();
    }

    public void OnClickLeft()
    {
        List<WeaponSO> haveList = userWeapon.GetHaveList();
        WeaponSO eqipWeapon = userWeapon.GetEqipWeapon();
        int index = haveList.FindIndex(n => n.code == eqipWeapon.code);

        index--;
        if (index < 0)
            index = haveList.Count - 1;

        userWeapon.ChangeEqipWeapon(index);

        Setting();
    }
}
