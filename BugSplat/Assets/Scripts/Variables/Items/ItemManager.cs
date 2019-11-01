using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : GameLoop
{

    public List<ItemObject> AllItems;
    public IntVariable BodyParts;

    public Inventory PlayerInventory;
    public FloatVariableList PlayerStats;
    public List<ShopItemSlot> ShopItemSlots;
    public AbilityManager AM;
   
    
    public override void LoopLateUpdate(float deltaTime)
    {
    }

    public override void LoopUpdate(float deltaTime)
    {
    }


}
