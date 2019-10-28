using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : GameLoop
{
    public List<ItemObject> items;
    
    // ModifiedStats are considered as flat atm
    public List<FloatVariable> ModifiedStats;
    public AbilityManager AM;

    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public override void LoopUpdate(float deltaTime)
    {
    }



    public void AddItem(ItemObject itemObj)
    {
        if (CanAddItem(itemObj))
            items.Add(itemObj);
    }


    bool CanAddItem(ItemObject itemObj)
    {
        if (itemObj.IsStackable || items.Count == 0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].name == itemObj.name)
                    Debug.Log("Player already has the item: " + itemObj.name);
                return false;
            } 
        }
        return true;
    }

    void Reset()
    {
        items.Clear();
        for (int i = 0; i < ModifiedStats.Count; i++)
        {
            // ModifiedStats are considered as flat
            ModifiedStats[i].Value = 0;
        }
    }
}
