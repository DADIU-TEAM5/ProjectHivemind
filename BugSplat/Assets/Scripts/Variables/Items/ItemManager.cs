using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void ChangeStats(ItemObject itemObj)
    {
        
        //Very Nice and Clean
        ModifiedStats[0].Value += itemObj.Flat_AttackDamage;
        ModifiedStats[1].Value += itemObj.Flat_AttackSpeed;
        ModifiedStats[2].Value += itemObj.Flat_Attack_Angle;
       
        ModifiedStats[3].Value += itemObj.Flat_DashSpeed;
        ModifiedStats[4].Value += itemObj.Flat_Dash_Length;
        ModifiedStats[5].Value += itemObj.Flat_MovementSpeed;

        ModifiedStats[6].Value += itemObj.Flat_Health;
        ModifiedStats[7].Value += itemObj.Flat_Damage_Reduction;
    }
}
