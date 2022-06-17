using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserWeapon : Singleton<UserWeapon>
{
    [AssetList(Path = "/SO/Weapon/")] public List<WeaponSO> weaponSOList = new List<WeaponSO>();
    const string eqipKey = "weaponEqip";
    const string haveKey = "weaponHave";

    public SpriteRenderer spriteRenderer;

    [Title("스크립트")]
    public CanvasManager canvasManager;
    public WeaponUI weaponUI;

    private void Start()
    {
        Setting();
    }

    public void Setting()
    {
        spriteRenderer.sprite = GetEqipWeapon().sprite;
    }

    public WeaponSO GetEqipWeapon()
    {
        return (PlayerPrefs.HasKey(eqipKey)) ? weaponSOList.Find(n => n.code == PlayerPrefs.GetString(eqipKey)) : weaponSOList[0];
    }

    public List<WeaponSO> GetHaveList()
    {
        List<WeaponSO> haveList = new List<WeaponSO>();
        haveList.Add(weaponSOList[0]);

        for (int i = 1; i < weaponSOList.Count; i++)
        {
            string code = weaponSOList[i].code;
            if (PlayerPrefs.HasKey(haveKey + code))
            {
                haveList.Add(weaponSOList[i]);
            }
        }

        return haveList;
    }

    [Button("PushWeapon")]
    public void PushWeapon(string _code)
    {
        WeaponSO weaponSO = weaponSOList.Find(n => n.code == _code);
        if (weaponSO == null)
        {
            Debug.LogError("정보가 잘못 되었음 코드 : " + _code);
            return;
        }

        PlayerPrefs.SetInt(haveKey + _code, 0);

        if (canvasManager.canvasStatus == CanvasStatus.로비)
        {
            weaponUI.CardDestroy();
            weaponUI.Setting();
        }
    }

    public void ChangeEqipWeapon(string _code)
    {
        WeaponSO weaponSO = GetHaveList().Find(n => n.code == _code);

        PlayerPrefs.SetString(eqipKey, weaponSO.code);
        Setting();
    }
}
