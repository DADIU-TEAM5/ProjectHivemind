using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesTrackUI : GameLoop
{
    public IntVariable TotalEnemyCount;
    public EnemyObjectList EnemiesListSO;
    public IntVariable EnemiesKilledSO;
    public IntVariable NumberOfWavesSO;
    public TextMeshProUGUI EKills;
    public TextMeshProUGUI EnemiesAtStart;
    public GameEvent HasWonEvent;
    public MapGenerator MapGen;
    public IntVariable CurrentLevel;
    private ShopLevels _levels;
    private int WaveCount = 0;
    private int eventRaisedCount;
    bool isWave;

    private void Start()
    {
        // Yes... it looks retarded
        MapGen = FindObjectOfType<MapGenerator>();
        _levels = MapGen.Levels;
        isWave = !_levels.LevelTierPicker[CurrentLevel.Value].IsGauntlet;

        eventRaisedCount = 0;
        EnemiesKilledSO.Value = 0;
        TotalEnemyCount.Value = 0;

        int enemytotal = EnemiesListSO.Items.Count + TotalEnemyCount.Value;
        EnemiesAtStart.text = enemytotal.ToString();
    }

    public override void LoopUpdate(float deltaTime)
    {
        EKills.text = EnemiesKilledSO.Value.ToString();
    }

    public override void LoopLateUpdate(float deltaTime) { }

    public void UpdateKills()
    {
        Debug.Log("WaveCount: " + WaveCount + ", WaveSO: " + NumberOfWavesSO.Value);
        EnemiesKilledSO.Value++;
        if (EnemiesKilledSO.Value >= TotalEnemyCount.Value && (!isWave || WaveCount+1 >= NumberOfWavesSO.Value))
        {
            HasWonEvent.Raise();
        }
    }

    public void UpdateEnemyCount()
    {
        Debug.Log("Enemy Spawned call");
        int enemytotal = TotalEnemyCount.Value;


        EnemiesAtStart.text = enemytotal.ToString();

    }

    public void NewWave()
    {
        if (WaveCount+1 < NumberOfWavesSO.Value)
        {
            Debug.Log("New Wave " + WaveCount);
            if (EnemiesKilledSO.Value > 0)
                WaveCount++;
            TotalEnemyCount.Value -= EnemiesKilledSO.Value;
            EnemiesKilledSO.Value = 0;
        }

    }
}
