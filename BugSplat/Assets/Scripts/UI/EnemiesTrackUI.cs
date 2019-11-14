using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesTrackUI : GameLoop
{
    public EnemyObjectList EnemiesListSO;
    public IntVariable EnemiesKilledSO;
    public TextMeshProUGUI EKills;
    public TextMeshProUGUI EnemiesAtStart;


    private void Start()
    {
        EnemiesKilledSO.Value = 0;

        EnemiesAtStart.text = EnemiesListSO.Items.Count.ToString();
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
        EnemiesAtStart.text = EnemiesListSO.Items.Count.ToString();
            
    }
}
