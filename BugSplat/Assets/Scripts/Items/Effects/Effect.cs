using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public EffectType EffectType;

    public abstract void Trigger(GameObject target = null);
    public abstract void Init();

    protected static EmptyMono MakeCoroutineObject() {
        var coroutineObject = new GameObject();
        var emptymonoObj = coroutineObject.AddComponent<EmptyMono>();
        coroutineObject.SetActive(true);

        return emptymonoObj;
    }
}
