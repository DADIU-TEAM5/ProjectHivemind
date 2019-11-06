using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public EffectType EffectType;

    public abstract void Trigger(GameObject target = null);
}
