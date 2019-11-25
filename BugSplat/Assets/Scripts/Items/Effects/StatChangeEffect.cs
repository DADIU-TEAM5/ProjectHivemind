using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Stat Effect")]
public class StatChangeEffect : Effect
{
    public FloatVariable Stat;
    public float Change;

    public bool ApplicableIgnore = false;

    public override void Init()
    {
    }

    public override void DoEffect(GameObject effectTarget = null)
    {
        Stat.SetValue(Stat.Value + Change);
    }

    public virtual float GetMax() => Stat.Max;

    public override int CanBeApplied() {
        if (Change <= 0 || ApplicableIgnore) return 0;

        if (Stat.Value >= GetMax()) {
            return 0;
        }

        return 1;
    }
}
