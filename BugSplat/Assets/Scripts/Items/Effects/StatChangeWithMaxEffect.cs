using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Effects/Stat Effect w/ Max reference")]
public class StatChangeWithMaxEffect : StatChangeEffect
{
    public FloatVariable MaxStat;

    public override int CanBeApplied() {
        if (Change < 0 || ApplicableIgnore) return 0;
        
        if (Stat.Value == MaxStat.Value) {
            return 0;
        }

        return 1;
    }
}
