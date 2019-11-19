using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public EffectType EffectType;

    public void Trigger(GameObject target = null) {
        if (CanBeApplied()) {
            DoEffect(target);
        }
    }

    public abstract void DoEffect(GameObject target);

    public abstract void Init();

    public virtual bool CanBeApplied() => true;

    protected static EmptyMono MakeCoroutineObject() {
        var coroutineObject = new GameObject();
        var emptymonoObj = coroutineObject.AddComponent<EmptyMono>();
        coroutineObject.SetActive(true);

        return emptymonoObj;
    }
}
