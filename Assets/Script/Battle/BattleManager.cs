using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    [AssetList(Path = "SO/Battle")]
    public List<BattleSO> battleSOs = new List<BattleSO>();

    public string battleKey = "Battle";

    bool isBattle = false;
    bool isWave = false;
    [HideInInspector] public BattleSO c_battleSO;
    int waveIndex = 0;

    public CanvasManager canvasManager;
    public Player player;
    public EnemySpawn enemySpawn;
    public BulletSpawn bulletSpawn;

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
        if (waveSOs.Count <= waveIndex)
        {
            BattleClear();
            return;
        }

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

        // 다음 웨이브 
        waveIndex++;
        WaveSetting();

        // 유저 최대체력 상승
        int waveHPAbility = UserAbility.Instance.GetAbility(Ability.웨이브마다최대체력상승);
        UserAbility.Instance.BuffAbility(new AbilityData(Ability.체력, waveHPAbility));
        Player.Instance.HP_Setting();
    }
    public void BattleClear()
    {
        BattleEnd();
        TextMessage.Instance.Show(
                 Language.Instance.GetScript(c_battleSO.nameCode) + "를 클리어 하셨습니다!"
                 , () => {
                 });

        // 카운트 추가 
        int count = PlayerPrefs.GetInt(battleKey + c_battleSO.code);
        PlayerPrefs.SetInt(battleKey + c_battleSO.code, count + 1);

        // unLock
        if (!PlayerPrefs.HasKey(battleKey + c_battleSO.unLockBattleCode))
        {
            PlayerPrefs.SetInt(battleKey + c_battleSO.unLockBattleCode, 0);
        }
    }
    public void BattleFailure()
    {
        BattleEnd();
        TextMessage.Instance.Show(
                 Language.Instance.GetScript(c_battleSO.nameCode) + "를 실패 하셨습니다"
                 , () => {
                 });
    }

    void BattleEnd()
    {
        isBattle = false;
        enemySpawn.AllDestroy();
        bulletSpawn.AllDestroy();
        if (waveSequence != null)
        {
            waveSequence.Kill();
        }
        CoreUI.Instance.Setting();
        player.BattleEnd(()=> { canvasManager.LobbySet(); });
    }
}
