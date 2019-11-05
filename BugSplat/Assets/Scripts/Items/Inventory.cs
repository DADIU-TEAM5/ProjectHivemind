using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> Items;

    public void AddItem(Item itemObj)
    {
    }
    
    private void OnEnable()
    {
        if (Items==null)
            Items = new List<Item>();
    }
}