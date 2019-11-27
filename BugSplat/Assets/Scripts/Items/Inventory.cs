using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> Items;

    public void AddItem(Item itemObj)
    {
        if (itemObj == null) return;

        Items.Add(itemObj);

        // Handle stat change effects
        for (var i = 0; i < itemObj.Effects.Length; i++) {
            var effect = itemObj.Effects[i];
            
            if (effect is StatChangeEffect) {
                effect.Trigger();
            }
        }
    }


    public void Reset()
    {
        if (Items == null) Items = new List<Item>();
    }

}