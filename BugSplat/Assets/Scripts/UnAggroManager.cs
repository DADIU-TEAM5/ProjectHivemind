using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnAggroManager : GameLoop
{
    //Use A slower GameLoop for better performance;
    public AggroCounter AC;
    public EnemyObjectList EnemyList;
    public UpdateManager UM;
    public IntVariable TimeBeforeResetAggro;
    
    private List<int> countdownTimers;

    public void Start()
    {
        UM = FindObjectOfType<UpdateManager>();
        countdownTimers = new List<int>();
    }

    public override void LoopLateUpdate(float deltaTime)
    {

    }

    public override void LoopUpdate(float deltaTime)
    {
        int i = 0;
        foreach (var enemy in EnemyList.Items)
        {
            if (countdownTimers.Count <= i)
            {
                countdownTimers.Add(TimeBeforeResetAggro.Value);
            }

            
            if(enemy != null && enemy.PlayerDetected)
            {

                if (!enemy.IsVisible())
                {
                    Debug.Log("Aggroed Enemy is out of vision!");

                    //UnAggro CountDown
                    countdownTimers[i]--;

                    if (countdownTimers[i] < 1)
                    {

                        enemy.PlayerDetected = false;
                        
                        AC.EnemyUnaggro();
                    }


                }
                else
                    countdownTimers[i] = TimeBeforeResetAggro.Value;
                //Vector3 worldPos = Cam.WorldToViewportPoint(enemy.transform.position);

                //if(worldPos.x < 1 && worldPos.x > 0 && worldPos.y < 1 && worldPos.y > 0 && worldPos.z > 0)
                //{
                //    // UnAggro CountDown
                //    Debug.Log("Aggroed Enemy is out of vision!");
                //}

            }
            i++;
        }
    }
}
