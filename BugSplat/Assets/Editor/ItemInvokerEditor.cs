using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemInvoker))]
public class ItemInvokerEditor : Editor
{
    public EffectType StatChangeEffectType;
    private ItemInvoker _invoker;

    void OnEnable() {
        _invoker = (ItemInvoker) target;
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Apply Stat change")) {
            _invoker.SetCurrentEffectType(StatChangeEffectType);
            _invoker.TriggerEffects(_invoker.gameObject);
        }
    }
}
