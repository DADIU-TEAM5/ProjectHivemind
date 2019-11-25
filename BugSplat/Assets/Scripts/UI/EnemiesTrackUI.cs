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
    public TextMeshProUGUI EKills;
    public TextMeshProUGUI EnemiesAtStart;

    private int eventRaisedCount;

    private void Start()
    {
        eventRaisedCount = 0;
        EnemiesKilledSO.Value = 0;

        int enemytotal = EnemiesListSO.Items.Count + TotalEnemyCount.Value;
        EnemiesAtStart.text = enemytotal.ToString();
    }

    public override void LoopUpdate(float deltaTime)
    {
        EKills.text = EnemiesKilledSO.Value.ToString();
    }

    public override void LoopLateUpdate(float deltaTime) {}

    public void UpdateKills()
    {
        EnemiesKilledSO.Value++;
    }

    public void UpdateEnemyCount()
    {
        eventRaisedCount++;
        if(eventRaisedCount <= EnemiesListSO.Items.Count)
        {
            Debug.Log("Enemy Spawned call");
            int enemytotal = EnemiesListSO.Items.Count + TotalEnemyCount.Value + EnemiesKilledSO.Value;


            EnemiesAtStart.text = enemytotal.ToString();

        }

    }
}
