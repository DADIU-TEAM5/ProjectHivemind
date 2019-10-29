using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName="Shop/ItemSlot")]
public class ShopItemSlot : ShopSlot
{
    public ItemPool Pool;
    public ShopLevels ShopLevels;
    public IntVariable CurrentLevel;

    private ItemObject Item = null;

   
    // TODO: NEED REFERENCE TO PLAYER ITEMS INVENTORY THING

    public override void OnPurchase()
    {
        if (Item == null) return;

        // Add item to player's inventory

        // Remove item    
        Item = null;
    }

    public void GetItemFromItemPool() {
        var shopLevel = ShopLevels.LevelTierPicker[CurrentLevel.Value];
        var decidedTier = shopLevel.ChooseTier();
        var decidedItem = Pool.GetItem(decidedTier);

        Item = decidedItem;
    }

    public ItemObject GetItem() => Item;

    void OnEnable() {
        if (Item == null)
            GetItemFromItemPool();
        else {
            Pool.ReplenishOnce(Item);
            GetItemFromItemPool();
        }
    } 


    void OnDisable() {
        if (Item != null) {
            Pool.ReplenishOnce(Item);
            Item = null;
        }
    }

    public override Sprite GetSprite()
    {
        // Return the item's sprite
        throw new System.NotImplementedException();
    }

    public override int GetPrice() => Item.Price;
}
