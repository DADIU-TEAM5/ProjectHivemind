using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AggroCounter : MonoBehaviour
{
    public IntVariable AggroedEnemies;
    public Text AggroTest;



    public void Start()
    {
        AggroedEnemies.Value = 0;
        AggroTest.text = AggroedEnemies.Value.ToString();
    }

    public void EnemyKilled()
    {

        // If PlayerDetected 
        if (AggroedEnemies.Value > 0)
            AggroedEnemies.Value--;

        if (AggroTest != null)
            AggroTest.text = AggroedEnemies.Value.ToString();


    }

    public void EnemyAggroed()
    {
        AggroedEnemies.Value++;
        if (AggroTest != null)
            AggroTest.text = AggroedEnemies.Value.ToString();
    }


}
