using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasManager : MonoBehaviour
{
    public CanvasStatus canvasStatus = CanvasStatus.타이틀;

    public List<GameObject> titleObjs = new List<GameObject>(); public GameObject titleObj;
    public List<GameObject> lobbyObjs = new List<GameObject>(); public GameObject lobbyObj;
    public List<GameObject> battleObjs = new List<GameObject>(); public GameObject battleObj;

    Dictionary<CanvasStatus, List<GameObject>> dics = new Dictionary<CanvasStatus, List<GameObject>>();

    [Title("스크립트")]
    public WeaponUI weaponUI;
    public UserAbility userAbility;

    [Title("오디오")]
    public AudioSource lobbyBGM;
    public AudioSource battleBGM;

    private void Start()
    {
        dics.Add(CanvasStatus.타이틀, titleObjs);
        dics.Add(CanvasStatus.로비, lobbyObjs);
        dics.Add(CanvasStatus.배틀, battleObjs);
    }

    bool flag = false;
    float fadeTime = 0.35f;

    [Button("TitleSet")]
    public void TitleSet()
    {
        if (flag)
            return;

        titleObj.SetActive(true);
        lobbyObj.SetActive(false);
        battleObj.SetActive(false);

        flag = true;

        weaponUI.CardDestroy();

        Function.Tool.FadeEffect(dics[canvasStatus], true, fadeTime, false, () => {

            for (int i = 0; i < dics[canvasStatus].Count; i++)
                dics[canvasStatus][i].SetActive(false);

            for (int i = 0; i < titleObjs.Count; i++)
                titleObjs[i].SetActive(true);

            Function.Tool.FadeEffect(titleObjs, false, fadeTime, false,()=> {
                canvasStatus = CanvasStatus.타이틀;
                flag = false;
            });
        });
    }
    [Button("LobbySet")]
    public void LobbySet()
    {
        if (flag)
            return;

        titleObj.SetActive(false);
        lobbyObj.SetActive(true);
        battleObj.SetActive(false);

        flag = true;

        Function.Tool.FadeEffect(dics[canvasStatus], true, fadeTime, false, () => {

            for (int i = 0; i < dics[canvasStatus].Count; i++)
                dics[canvasStatus][i].SetActive(false);

            for (int i = 0; i < lobbyObjs.Count; i++)
                lobbyObjs[i].SetActive(true);

            Function.Tool.FadeEffect(lobbyObjs, false, fadeTime, false, () => {
                canvasStatus = CanvasStatus.로비;
                flag = false;

                weaponUI.Setting();
            });
        });

        Camera.main.DOOrthoSize(5, fadeTime);

        SoundManager.Instance.BGMSoundPlay(lobbyBGM);
    }
    [Button("BattleSet")]
    public void BattleSet()
    {
        if (flag)
            return;

        titleObj.SetActive(false);
        lobbyObj.SetActive(false);
        battleObj.SetActive(true);

        userAbility.BattleSetting();

        flag = true;

        weaponUI.CardDestroy();

        Function.Tool.FadeEffect(dics[canvasStatus], true, fadeTime, false, () => {

            for (int i = 0; i < dics[canvasStatus].Count; i++)
                dics[canvasStatus][i].SetActive(false);

            for (int i = 0; i < battleObjs.Count; i++)
                battleObjs[i].SetActive(true);

            Function.Tool.FadeEffect(battleObjs, false, fadeTime, false, () => {
                canvasStatus = CanvasStatus.배틀;
                flag = false;
            });
        });

        Camera.main.DOOrthoSize(8, fadeTime);

        SoundManager.Instance.BGMSoundPlay(battleBGM);
    }
   
}

public enum CanvasStatus
{
    타이틀,
    로비,
    배틀
}