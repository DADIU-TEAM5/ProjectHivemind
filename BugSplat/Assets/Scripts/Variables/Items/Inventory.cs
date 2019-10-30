using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    public List<ItemObject> Items;

    private void OnEnable()
    {
        if (Items==null)
        Items = new List<ItemObject>();
    }
}
