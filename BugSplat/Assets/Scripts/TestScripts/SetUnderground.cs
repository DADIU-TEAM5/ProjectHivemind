using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUnderground : MonoBehaviour
{

    public Enemy TargetEnemy;


    public void SetUndergroundBool(int chosenValue)
    {
        if (TargetEnemy == null)
        {
            TargetEnemy = this.gameObject.GetComponentInParent<Enemy>();
        }

        if (chosenValue == 0)
        {
            TargetEnemy.IsUnderground = false;
        } else
        {
            TargetEnemy.IsUnderground = true;
        }
 
    }
}
