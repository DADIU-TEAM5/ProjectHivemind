using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Effects/Stat Effect w/ Max reference")]
public class StatChangeWithMaxEffect : StatChangeEffect
{
    public FloatVariable MaxStat;

    public override bool CanBeApplied() {
        if (Stat.Value == MaxStat.Value) {
            return false;
        }

        return true;
    }
}