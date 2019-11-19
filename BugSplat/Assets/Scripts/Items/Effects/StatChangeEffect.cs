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

    public override bool CanBeApplied() {
        if (Stat.Value == Stat.Max) {
            return false;
        }

        return true;
    }
}