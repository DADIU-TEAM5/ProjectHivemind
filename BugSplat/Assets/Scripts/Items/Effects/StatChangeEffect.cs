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

    public override void Trigger(GameObject effectTarget = null)
    {
        Stat.SetValue(Stat.Value + Change);
    }
}