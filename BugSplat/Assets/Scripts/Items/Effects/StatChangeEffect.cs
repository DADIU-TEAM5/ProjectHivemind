using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Stat Effect")]
public class StatChangeEffect : Effect
{
    public FloatVariable Stat;
    public float Change;

    public override void Init()
    {
    }

    public override void DoEffect(GameObject effectTarget = null)
    {
        Stat.SetValue(Stat.Value + Change);
    }

    public override int CanBeApplied() {
        if (Change < 0) return 0;

        if (Stat.Value == Stat.Max) {
            return -1;
        }

        return 1;
    }
}