using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemiesTrackUI : GameLoop
{
    public GameObjectList EnemiesListSO;
    public IntVariable EnemiesKilledSO;
    public TextMeshProUGUI EKills;
    public TextMeshProUGUI ELeft;


    public override void LoopUpdate(float deltaTime)
    {
        EKills.text = EnemiesKilledSO.Value.ToString();

        ELeft.text = EnemiesListSO.Items.Count.ToString();
    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }

    public void UpdateKills()
    {
        EnemiesKilledSO.Value++;
    }
}
