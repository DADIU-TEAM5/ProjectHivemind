using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemInvoker : MonoBehaviour
{
    public Inventory Inventory;

    public EffectType StatChange;

    private Dictionary<EffectType, List<Effect>> Effects;

    public void Awake() {
        Effects = new Dictionary<EffectType, List<Effect>>();
    }

    public void Start() {
       LoadInventory(); 
    }

    private void LoadInventory() {
        for (var i = 0; i < Inventory.Items.Count; i++) {
            var item = Inventory.Items[i];

            for (var j = 0; j < item.Effects.Length; j++) {
                var effect = item.Effects[j];

                if (!Effects.ContainsKey(effect.EffectType)) {
                    Effects[effect.EffectType] = new List<Effect> { effect };
                } else {
                    Effects[effect.EffectType].Add(effect);
                }
            }
        }
    }

    public void TriggerEffects(EffectType type) {
        if (!Effects.ContainsKey(type)) return;

        var effects = Effects[type];

        foreach (var effect in effects) {
            effect.Trigger();
        }
    }
}