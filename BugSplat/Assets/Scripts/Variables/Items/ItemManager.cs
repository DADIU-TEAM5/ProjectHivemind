using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : GameLoop
{
    public List<ItemObject> Items;
    public Inventory PlayerInventory;

    // ModifiedStats are considered as flat atm
    public FloatVariableList PlayerStats;
    public List<FloatVariable> ModifiedStats;
    public AbilityManager AM;
    public List<ItemObject> AllItems;
    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public override void LoopUpdate(float deltaTime)
    {
    }


    public void AddItem(ItemObject itemObj)
    {
        if (CanAddItem(itemObj))
        {
            Items.Add(itemObj);
            ChangeStats(itemObj);
            PlayerInventory.Items.Add(itemObj);
        }

    }


    bool CanAddItem(ItemObject itemObj)
    {
        if (itemObj.IsStackable || PlayerInventory.Items.Count == 0)
        {
            return true;
        }
        else
        {
            for (int i = 0; i < PlayerInventory.Items.Count; i++)
            {
                if (PlayerInventory.Items[i] != null && PlayerInventory.Items[i].name == itemObj.name)
                {
                    Debug.Log("Player already has the item: " + itemObj.name);
                    return false;

                }
            }
        }
        return true;
    }

    public void ResetItems()
    {
        PlayerInventory.Items.Clear();
        Items.Clear();
        for (int i = 0; i < PlayerStats.Value.Count; i++)
        {
            // ModifiedStats are considered as flat
            if (PlayerStats.Value[i] != null)
                PlayerStats.Value[i].ResetValue();
        }
    }

    void ChangeStats(ItemObject itemObj)
    {

        for (int i = 0; i < PlayerStats.Value.Count; i++)
        {
            PlayerStats.Value[i].Value += itemObj.FlatStatChanges[i];

        }
    }
}
