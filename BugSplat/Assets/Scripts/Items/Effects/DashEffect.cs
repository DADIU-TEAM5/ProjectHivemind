using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Effects/Dash Init")]
public class DashEffect : Effect
{
    public Vector3 StartPos = Vector3.zero;

    public override void Init()
    {
    }

    public override void DoEffect(GameObject target = null)
    {
        StartPos = target.transform.position;
    }
}
