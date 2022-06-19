using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    public List<BattleSO> battleSOs = new List<BattleSO>();

    public string battleKey = "Battle";

    bool isBattle = false;
    bool isWave = false;
    [HideInInspector] public BattleSO c_battleSO;
    [HideInInspector] public int waveIndex = 0;

    public CanvasManager canvasManager;
    public Player player;
    public EnemySpawn enemySpawn;
    public BulletSpawn bulletSpawn;
    public ItemSelect itemSelect;
    public UserItem userItem;
    public Satellite satellite;

    public Text difiText;
    public Text waveText;

    public AudioSource victoryAudio, failureAudio , unlockAudio;

    public void BattleStart(BattleSO _battleSO)
    {
        if (canvasManager.canvasStatus != CanvasStatus.로비)
        {
            return;
        }

        isBattle = true;
        c_battleSO = _battleSO;
        waveIndex = 0;
        difiText.text = Language.Instance.GetScript(_battleSO.nameCode);
        difiText.font = Language.Instance.GetFont();

        itemSelect.BattleStart();
        userItem.BattleSetting();
        player.BattleSetting();
        waveText.text = "wave " + (waveIndex + 1);

        canvasManager.BattleSet();
        TextMessage.Instance.Show(
            Language.Instance.GetScript(_battleSO.nameCode) + "전투 시작합니다"
            , () => {
                if (isBattle)
                {
                    WaveSetting();
                }
            });
    }
    Sequence waveSequence;
    void WaveSetting()
    {
        isWave = true;

        waveText.text = "wave " + (waveIndex + 1);

        List<WaveSO> waveSOs = c_battleSO.waveSOs;

        if (waveSequence != null)
        {
            waveSequence.Kill();
        }
        waveSequence = DOTween.Sequence();
        List<WaveEnemy> waveEnemies = waveSOs[waveIndex].waveEnemies;
        for (int i = 0; i < waveEnemies.Count; i++)
        {
            EnemySO enemySO = waveEnemies[i].enemySO;
            SpawnPoint spawnPoint = waveEnemies[i].spawnPoint;
            float waitTime = waveEnemies[i].spawnTime;
            waveSequence.InsertCallback(waitTime, () => {
                enemySpawn.Spawn(enemySO, c_battleSO, spawnPoint);
            });
        }

        waveSequence.InsertCallback(waveEnemies[waveEnemies.Count - 1].spawnTime, () => {
            isWave = false;
        });

        waveSequence.Play();
    }
    public void WaveCheck()
    {
        if (isWave)
        {
            // 아직 웨이브 중임
            return;
        }

        for (int i = 0; i < enemySpawn.spawnObjList.Count; i++)
        {
            if (enemySpawn.spawnObjList[i].activeSelf)
            {
                // 몬스터있음
                return;
            }
        }

        if (c_battleSO.waveSOs.Count - 1 <= waveIndex)
        {
            BattleClear();
        }
        else
        {
            itemSelect.Charge_Q();
            itemSelect.Show();
        }
    }
    public void NextWave()
    {
        // 다음 웨이브 
        waveIndex++;
        WaveSetting();

        // 유저 체력 상승
        int waveHPAbility = UserAbility.Instance.GetAbility(Ability.웨이브마다최대체력상승);
        int recoveryValue = (int)(waveHPAbility / 1000f * UserAbility.Instance.GetAbility(Ability.체력));
        Player.Instance.HP_Recovery(recoveryValue);

        if (player.isAutoCreate)
        {
            ItemSelect.Instance.PushNomal();
        }
        UserMove.Instance.idleTime = 0;
        UserMove.Instance.moveTime = 0;
    }
    public void BattleClear()
    {
        BattleEnd();

        SoundManager.Instance.FXSoundPlay(victoryAudio);

        ScreenShow.Instance.Show("success", () => {

            player.BattleEnd(() => { canvasManager.LobbySet(); });

            BattleSO nextBattle = GetBattleSO(c_battleSO.unLockBattleCode);
            if (nextBattle != null && !PlayerPrefs.HasKey(battleKey + c_battleSO.unLockBattleCode))
            {
                SoundManager.Instance.FXSoundPlay(unlockAudio);
                TextMessage.Instance.Show(
                Language.Instance.GetScript(nextBattle.nameCode) + "가 해금되었습니다!"
             , () => {
             });
            }

            // 카운트 추가 
            int count = PlayerPrefs.GetInt(battleKey + c_battleSO.code);
            PlayerPrefs.SetInt(battleKey + c_battleSO.code, count + 1);

            // unLock
            if (!PlayerPrefs.HasKey(battleKey + c_battleSO.unLockBattleCode))
            {
                PlayerPrefs.SetInt(battleKey + c_battleSO.unLockBattleCode, 0);
            }
        });
    }
    public void BattleFailure()
    {
        BattleEnd();

        SoundManager.Instance.FXSoundPlay(failureAudio);

        ScreenShow.Instance.Show("failure", () => {
            player.BattleEnd(() => { canvasManager.LobbySet(); });

            TextMessage.Instance.Show(
                     Language.Instance.GetScript(c_battleSO.nameCode) + "를 실패 하셨습니다"
                     , () => {
                     });
        });
    }
    void BattleEnd()
    {
        isBattle = false;

        if (waveSequence != null)
        {
            waveSequence.Kill();
        }

        enemySpawn.AllDestroy();
        bulletSpawn.AllDestroy();
        satellite.StatelliteStop();
        itemSelect.Close();
        DronSpawn.Instance.AllDestroy();
        StraightShot.Instance.End();
        ColdShot.Instance.End();
        MultipleShot.Instance.End();
    
        CoreUI.Instance.Setting();
    }
    public void OnClickExit()
    {
        Time.timeScale = 0;
        YesNoMessage.Instance.Show("로비로 돌아가시겠습니까?", () => {
            Time.timeScale = 1;
            BattleEnd();
            ScreenShow.Instance.Show("failure", () => {
                player.BattleEnd(() => { canvasManager.LobbySet(); });
            });
        },()=> {
            Time.timeScale = 1;
        });
    }

    BattleSO GetBattleSO(string _code)
    {
        return battleSOs.Find(n => n.code == _code);
    }

    [Button("Test")]
    public void Test()
    {
        for (int i = 0; i < battleSOs.Count; i++)
        {
            if (PlayerPrefs.HasKey(battleKey + battleSOs[i].unLockBattleCode))
            {
                Debug.Log("있다 : " + battleKey + battleSOs[i].unLockBattleCode);
            }
        }
    }
}
