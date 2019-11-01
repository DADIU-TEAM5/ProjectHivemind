using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    
    public List<ItemObject> Items;
    public FloatVariableList PlayerStats;

    private void OnEnable()
    {
        if (Items==null)
        Items = new List<ItemObject>();
    }

    public void AddItem(ItemObject itemObj)
    {
        if (CanAddItem(itemObj))
        {
            Items.Add(itemObj);
            ChangeStats(itemObj);
            
        }

    }

    public void ResetItems()
    {
        Items.Clear();
        for (int i = 0; i < PlayerStats.Value.Count; i++)
        {
            // ModifiedStats are considered as flat
            if (PlayerStats.Value[i] != null)
                PlayerStats.Value[i].ResetValue();
        }
    }

    bool CanAddItem(ItemObject itemObj)
    {
        if (itemObj.IsStackable || Items.Count == 0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] != null && Items[i].name == itemObj.name)
                {
                    Debug.Log("Player already has the item: " + itemObj.name);
                    return false;

                }
            }
        }
        return true;
    }

    void ChangeStats(ItemObject itemObj)
    {

        for (int i = 0; i < PlayerStats.Value.Count; i++)
        {
            if (PlayerStats.Value[i] != null)
                PlayerStats.Value[i].Value += itemObj.FlatStatChanges[i];

        }
    }
}
