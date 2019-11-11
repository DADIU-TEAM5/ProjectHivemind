using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemInvoker : MonoBehaviour
{
    public Inventory Inventory;

    public EffectType CurrentEffectType;

    private Dictionary<EffectType, List<Effect>> Effects;

    public void Awake() {
        Effects = new Dictionary<EffectType, List<Effect>>();
    }

    public void Start() {
       LoadInventory(); 
    }

    public void SetCurrentEffectType(EffectType type) {
        if (Effects.ContainsKey(type)) {
            CurrentEffectType = type;
        } else
        {
            CurrentEffectType = null;
        }
    }

    private void LoadInventory() {
        for (var i = 0; i < Inventory.Items.Count; i++) {
            var item = Inventory.Items[i];

            for (var j = 0; j < item.Effects.Length; j++) {
                var effect = item.Effects[j];
                Debug.Log(effect.name);
                effect.Init();
                if (!Effects.ContainsKey(effect.EffectType)) {
                    Effects[effect.EffectType] = new List<Effect>(10) { effect };
                } else {
                    Effects[effect.EffectType].Add(effect);
                }
            }
        }
    }

    public void TriggerEffects(GameObject target) {
        if (target == null || CurrentEffectType == null || !Effects.ContainsKey(CurrentEffectType)) return;

        var effects = Effects[CurrentEffectType];

        foreach (var effect in effects) {
            effect.Trigger(target);
        }
    }
}