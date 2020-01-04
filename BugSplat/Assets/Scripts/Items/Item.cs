using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName="Item/New Item")]
public class Item : ScriptableObject
{
    [HideLabel]
    [BoxGroup("Info")]
    public ItemInfo Info;

    [InlineEditor]
    public Effect[] Effects;

    public bool Purchasable() {
        int start = 0;
        for (var i = 0; i < Effects.Length; i++) {
            var effect = Effects[i];
            if (effect == null) continue;

            var purchasable = Effects[i].CanBeApplied();

            start |= purchasable;
        }

        return start > 0;
    }
}
