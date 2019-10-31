using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : GameLoop
{
    public List<ItemObject> Items;
    public Inventory PlayerInventory;

    // ModifiedStats are considered as flat atm
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
        for (int i = 0; i < ModifiedStats.Count; i++)
        {
            // ModifiedStats are considered as flat
            if (ModifiedStats[i] != null)
                ModifiedStats[i].Value = 0;
        }
    }

    void ChangeStats(ItemObject itemObj)
    {

        //Very Nice and Clean
        //ModifiedStats[0].Value += itemObj.Flat_AttackDamage;
        //ModifiedStats[1].Value += itemObj.Flat_AttackSpeed;
        //ModifiedStats[2].Value += itemObj.Flat_Attack_Angle;

        //ModifiedStats[3].Value += itemObj.Flat_DashSpeed;
        //ModifiedStats[4].Value += itemObj.Flat_Dash_Length;
        //ModifiedStats[5].Value += itemObj.Flat_MovementSpeed;

        //ModifiedStats[6].Value += itemObj.Flat_Health;
        //ModifiedStats[7].Value += itemObj.Flat_Damage_Reduction;
    }
}
