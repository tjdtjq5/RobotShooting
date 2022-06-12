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

        canvasManager.BattleSet();
        TextMessage.Instance.Show(
            Language.Instance.GetScript(_battleSO.nameCode) + " 전투를 시작합니다"
            , () => {
                player.BattleSetting();
                WaveSetting();

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
                enemySpawn.Spawn(enemySO, spawnPoint);
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
        Player.Instance.HP_Recovery(waveHPAbility);

        if (player.isAutoCreate)
        {
            ItemSelect.Instance.PushNomal();
        }

        BulletSpawn.Instance.AllDestroy();
        UserMove.Instance.idleTime = 0;
    }
    public void BattleClear()
    {
        BattleEnd();

        ScreenShow.Instance.Show("success", () => {

            player.BattleEnd(() => { canvasManager.LobbySet(); });

            if (!string.IsNullOrEmpty(c_battleSO.unLockBattleCode))
            {
                TextMessage.Instance.Show(
             Language.Instance.GetScript(c_battleSO.unLockBattleCode) + "가 해금되었습니다!"
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
        enemySpawn.AllDestroy();
        bulletSpawn.AllDestroy();
        satellite.StatelliteStop();
        itemSelect.Close();
        DronSpawn.Instance.AllDestroy();

        if (waveSequence != null)
        {
            waveSequence.Kill();
        }
        CoreUI.Instance.Setting();
    }
    public void OnClickExit()
    {
        Time.timeScale = 0;
        YesNoMessage.Instance.Show("로비로 돌아가시겠습니까?", () => {
            Time.timeScale = 1;
            BattleEnd();
            player.BattleEnd(() => { canvasManager.LobbySet(); });
        },()=> {
            Time.timeScale = 1;
        });
    }
}
