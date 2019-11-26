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
    public BoolVariable IsWaveSO;

    private int WaveCount = 0;
    private int eventRaisedCount;

    private void Start()
    {
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
        EnemiesKilledSO.Value++;
        if(EnemiesKilledSO.Value>=TotalEnemyCount.Value && (!IsWaveSO.Value || WaveCount > NumberOfWavesSO.Value))
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
        TotalEnemyCount.Value-= EnemiesKilledSO.Value;
        EnemiesKilledSO.Value = 0;
        WaveCount++;
    }
}
