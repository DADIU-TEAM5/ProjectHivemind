using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public EffectType EffectType;

    public void Trigger(GameObject target = null) {
        if (CanBeApplied() >= 0) {
            DoEffect(target);
        }
    }

    public abstract void DoEffect(GameObject target);

    public abstract void Init();

    public virtual int CanBeApplied() => 1;

    protected static EmptyMono MakeCoroutineObject() {
        var coroutineObject = new GameObject();
        var emptymonoObj = coroutineObject.AddComponent<EmptyMono>();
        coroutineObject.SetActive(true);

        return emptymonoObj;
    }
}
